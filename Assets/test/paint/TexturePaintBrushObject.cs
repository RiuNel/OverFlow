using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rito.TexturePainter
{
    [DisallowMultipleComponent]
    public class TexturePaintBrushObject : MonoBehaviour
    {
        public GameManager GameManager;
        // Public Fields
        [Range(0.01f, 1f)] public float brushSize = 0.1f;
        public Texture2D brushTexture;
        public Color brushColor = Color.white;

        // Private Fields
        private TexturePaintTarget paintTarget;
        private Collider prevCollider;
        private Texture2D CopiedBrushTexture;
        private Vector2 sameUvPoint;
        private Rigidbody Rigidbody;



        // �浹 �ð� ������ �ʵ�
        //private Dictionary<Collider, float> collisionTimeTracker = new Dictionary<Collider, float>();

        private void Awake()
        {
            if (brushTexture == null)
            {
                CreateDefaultBrushTexture();
            }
            CopyBrushTexture();
        }

        private void FixedUpdate()
        {
            //UpdateBrushColorOnEditor();

            if (Input.GetMouseButton(0) == false) return;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit))
            {
                Collider currentCollider = hit.collider;
                if (currentCollider != null)
                {
                    if (prevCollider == null || prevCollider != currentCollider)
                    {
                        prevCollider = currentCollider;
                        currentCollider.TryGetComponent(out paintTarget);
                    }

                    if (sameUvPoint != hit.textureCoord)
                    {
                        sameUvPoint = hit.textureCoord;
                        Vector2 pixelUV = hit.textureCoord;
                        pixelUV.x *= paintTarget.resolution;
                        pixelUV.y *= paintTarget.resolution;
                        paintTarget.DrawTexture(pixelUV.x, pixelUV.y, brushSize, CopiedBrushTexture);
                    }
                }
            }
        }

        private Vector2 GetUVCoordinate(TexturePaintTarget target, Vector3 hitPoint)
        {
            // �浹 ������ Ÿ�� �޽��� UV ��ǥ ����
            MeshCollider meshCollider = target.GetComponent<MeshCollider>();
            if (meshCollider == null) return Vector2.zero;

            RaycastHit hit;
            if (Physics.Raycast(hitPoint + Vector3.up * 0.1f, Vector3.down, out hit, 1f))
            {
                if (hit.collider == meshCollider)
                {
                    Vector2 pixelUV = hit.textureCoord;
                    pixelUV.x *= target.resolution;
                    pixelUV.y *= target.resolution;
                    return pixelUV;
                }
            }
            return Vector2.zero;
        }

        private void OnCollisionStay(Collision collision)
        {
            if (!GameManager.isGrab)
            {
                return;
            }

            // �浹�� ������Ʈ���� TexturePaintTarget ������Ʈ �˻�
            if (collision.collider.TryGetComponent(out TexturePaintTarget paintTarget))
            {
                // �浹 ���� ��������
                foreach (ContactPoint contact in collision.contacts)
                {
                    // UV ��ǥ ���
                    Vector2 pixelUV = GetUVCoordinate(paintTarget, contact.point);

                    // UV ��ǥ ��ȿ�� �˻� �� �׸� �׸���
                    if (pixelUV != sameUvPoint)
                    {
                        sameUvPoint = pixelUV; // �ߺ� ����
                        paintTarget.DrawTexture(pixelUV.x, pixelUV.y, brushSize, CopiedBrushTexture);
                    }
                }
            }
        }

        /*private void OnCollisionExit(Collision collision)
        {
            if (collisionTimeTracker.ContainsKey(collision.collider))
            {
                collisionTimeTracker.Remove(collision.collider);
            }
        }*/

        // �귯�� ���� ����
        public void SetBrushColor(in Color color)
        {
            brushColor = color;
            CopyBrushTexture();
        }

        // �⺻ ����(��)�� �귯�� �ؽ��� ����
        private void CreateDefaultBrushTexture()
        {
            int res = 512;
            float hRes = res * 0.5f;
            float sqrSize = hRes * hRes;

            brushTexture = new Texture2D(res, res,TextureFormat.Alpha8,true);
            brushTexture.filterMode = FilterMode.Point;
            //brushTexture.alphaIsTransparency = true;

            for (int y = 0; y < res; y++)
            {
                for (int x = 0; x < res; x++)
                {
                    float sqrLen = (hRes - x) * (hRes - x) + (hRes - y) * (hRes - y);
                    float alpha = Mathf.Max(sqrSize - sqrLen, 0f) / sqrSize;
                    brushTexture.SetPixel(x, y, new Color(1f, 1f, 1f, alpha));
                }
            }

            brushTexture.Apply();
        }

        // ���� �귯�� �ؽ��� -> ���� �귯�� �ؽ���(���� ����) ����
        private void CopyBrushTexture()
        {
            if (brushTexture == null) return;

            DestroyImmediate(CopiedBrushTexture);

            CopiedBrushTexture = new Texture2D(brushTexture.width, brushTexture.height, TextureFormat.Alpha8,true);
            CopiedBrushTexture.filterMode = FilterMode.Point;
            //CopiedBrushTexture.alphaIsTransparency = true;

            int height = brushTexture.height;
            int width = brushTexture.width;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color c = brushColor;
                    c.a *= brushTexture.GetPixel(x, y).a;
                    CopiedBrushTexture.SetPixel(x, y, c);

                    /*Color originalColor = brushTexture.GetPixel(x, y);
                    Color finalColor = originalColor * brushColor; // ���� ����
                    CopiedBrushTexture.SetPixel(x, y, finalColor);*/
                }
            }

            CopiedBrushTexture.Apply();
        }

        //        // �����Ϳ��� �귯�� ���� ������Ʈ
        //#if UNITY_EDITOR
        //        private Color prevBrushColor;
        //        private float brushTextureUpdateCounter = 0f;
        //        private const float BrushTextureUpdateCounterInitValue = 0.7f;
        //        private void OnValidate()
        //        {
        //            if (Application.isPlaying && prevBrushColor != brushColor)
        //            {
        //                brushTextureUpdateCounter = BrushTextureUpdateCounterInitValue;
        //                prevBrushColor = brushColor;
        //            }
        //        }
        //#endif
        //[System.Diagnostics.Conditional("UNITY_EDITOR")]
        //private void UpdateBrushColorOnEditor()
        //{
        //    if (brushTextureUpdateCounter > 0f &&
        //        brushTextureUpdateCounter <= BrushTextureUpdateCounterInitValue)
        //    {
        //        brushTextureUpdateCounter -= Time.deltaTime;
        //    }

        //    if (brushTextureUpdateCounter < 0f)
        //    {
        //        CopyBrushTexture();
        //        brushTextureUpdateCounter = 9999f;
        //    }
        //}
    }
}
