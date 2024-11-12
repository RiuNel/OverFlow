using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rito.TexturePainter
{
    [DisallowMultipleComponent]
    public class TexturePaintBrushObject : MonoBehaviour
    {
        // Public Fields
        [Range(0.01f, 1f)] public float brushSize = 0.1f;
        public Texture2D brushTexture;
        public Color brushColor = Color.white;

        // Private Fields
        private TexturePaintTarget paintTarget;
        private Collider prevCollider;
        private Texture2D CopiedBrushTexture;
        private Vector2 sameUvPoint;

        // 충돌 시간 추적용 필드
        private float collisionStartTime = -1f;
        public float limitTime = 2f;

        private void Awake()
        {
            if (brushTexture == null)
            {
                CreateDefaultBrushTexture();
            }
            CopyBrushTexture();
        }

        private void Update()
        {
            
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag != "GarbageBag") return;
            Debug.Log("충돌 시작");
            // 충돌 시작 시간을 기록
            collisionStartTime = Time.time;
        }

        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.tag != "GarbageBag") return;
            foreach (ContactPoint contact in collision.contacts)
            {
                //Debug.Log(contact.point);
                PaintAtCollisionPoint(contact.point, collision.collider);
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.tag != "GarbageBag") return;
            Debug.Log("충돌 끝");

            // 2초 이상
            if (collisionStartTime > 0 && (Time.time - collisionStartTime >= limitTime))
            {
                Debug.Log("2초 후");
                GameManager.instance.isPaint = 1;
            }
            // 2초 이하
            if (collisionStartTime > 0 && (Time.time - collisionStartTime < limitTime))
            {
                Debug.Log("2초 전");
                GameManager.instance.isPaint = 2;
            }
            

            // 충돌 종료 시 기록 초기화
            collisionStartTime = -1f;
        }

        private void PaintAtCollisionPoint(Vector3 hitPoint, Collider collider)
        {
            if (collider.CompareTag("GarbageBag"))
            {
                if (collider != prevCollider)
                {
                    prevCollider = collider;
                    collider.TryGetComponent(out paintTarget);
                }

                if (paintTarget != null)
                {
                    Vector2 uv;
                    Ray ray = new Ray(hitPoint, transform.forward);
                    if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider == collider)
                    {
                        uv = hit.textureCoord;
                        uv.x *= paintTarget.resolution;
                        uv.y *= paintTarget.resolution;

                        if (sameUvPoint != uv)
                        {
                            sameUvPoint = uv;
                            paintTarget.DrawTexture(uv.x, uv.y, brushSize, CopiedBrushTexture);
                        }
                    }
                }
            }
        }

        // 브러시 색상 변경
        public void SetBrushColor(in Color color)
        {
            brushColor = color;
            CopyBrushTexture();
        }

        // 기본 형태(원)의 브러시 텍스쳐 생성
        private void CreateDefaultBrushTexture()
        {
            int res = 512;
            float hRes = res * 0.5f;
            float sqrSize = hRes * hRes;

            brushTexture = new Texture2D(res, res);
            brushTexture.filterMode = FilterMode.Point;
            brushTexture.alphaIsTransparency = true;

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

        // 원본 브러시 텍스쳐 -> 실제 브러시 텍스쳐(색상 적용) 복제
        private void CopyBrushTexture()
        {
            if (brushTexture == null) return;

            DestroyImmediate(CopiedBrushTexture);

            CopiedBrushTexture = new Texture2D(brushTexture.width, brushTexture.height);
            CopiedBrushTexture.filterMode = FilterMode.Point;
            CopiedBrushTexture.alphaIsTransparency = true;

            int height = brushTexture.height;
            int width = brushTexture.width;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color c = brushColor;
                    c.a *= brushTexture.GetPixel(x, y).a;
                    CopiedBrushTexture.SetPixel(x, y, c);
                }
            }

            CopiedBrushTexture.Apply();
        }

        // 에디터에서 브러시 색상 업데이트
#if UNITY_EDITOR
        private Color prevBrushColor;
        private float brushTextureUpdateCounter = 0f;
        private const float BrushTextureUpdateCounterInitValue = 0.7f;
        private void OnValidate()
        {
            if (Application.isPlaying && prevBrushColor != brushColor)
            {
                brushTextureUpdateCounter = BrushTextureUpdateCounterInitValue;
                prevBrushColor = brushColor;
            }
        }
#endif
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        private void UpdateBrushColorOnEditor()
        {
            if (brushTextureUpdateCounter > 0f &&
                brushTextureUpdateCounter <= BrushTextureUpdateCounterInitValue)
            {
                brushTextureUpdateCounter -= Time.deltaTime;
            }

            if (brushTextureUpdateCounter < 0f)
            {
                CopyBrushTexture();
                brushTextureUpdateCounter = 9999f;
            }
        }
    }
}
