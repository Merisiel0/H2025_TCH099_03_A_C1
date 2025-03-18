using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// You want to understand? Either prepare to meet god or watch this video.
/// https://youtu.be/wUDqSXsTY2g
/// </summary>

public class WallShadowCamera : MonoBehaviour
{
    [SerializeField] private Material GLDraw;

    private GameObject _player;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
    }

    void OnDestroy()
    {
        RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
    }

    private void OnEndCameraRendering(ScriptableRenderContext context, Camera cam)
    {
        float camHeight = cam.orthographicSize * 2; // 5
        float camWidth = camHeight * cam.aspect; // 8.88888 close to 9

        float cameraLeft = cam.transform.position.x - camWidth / 2; // -8.8888
        float cameraBottom = cam.transform.position.y - camHeight / 2; // -5

        GL.PushMatrix();
        GLDraw.SetPass(0);
        GL.LoadOrtho();

        Collider2D[] collisions = Physics2D.OverlapBoxAll(cam.transform.position, new Vector2(camWidth, camHeight), 0, LayerMask.GetMask("Wall"));

        foreach (Collider2D collision in collisions)
        {
            Wall wall = collision.GetComponent<Wall>();

            // put wall coordinates in camera space
            float left = (wall.left - cameraLeft) / camWidth; // (1.5 - -8.8888) / 8.8888
            float top = (wall.top - cameraBottom) / camHeight;
            float right = (wall.right - cameraLeft) / camWidth;
            float bottom = (wall.bottom - cameraBottom) / camHeight;

            if (_player.transform.position.x <= wall.center.x && _player.transform.position.y <= wall.center.y)
            {
                DrawShadow(left, bottom, right, top);
            }
            else if (_player.transform.position.x <= wall.center.x && _player.transform.position.y >= wall.center.y)
            {
                DrawShadow(left, top, right, bottom);
            }
            else if (_player.transform.position.x >= wall.center.x && _player.transform.position.y >= wall.center.y)
            {
                DrawShadow(right, top, left, bottom);
            }
            else if (_player.transform.position.x >= wall.center.x && _player.transform.position.y <= wall.center.y)
            {
                DrawShadow(right, bottom, left, top);
            }
        }

        GL.PopMatrix();
    }

    void DrawShadow(float x1, float y1, float x2, float y2)
    {
        float x = 0.5f;
        float y = 0.5f;
        int projectedLength = 100;

        float projx1 = x2 + (x2 - x) * projectedLength;
        float projy1 = y1 + (y1 - y) * projectedLength;

        float projx2 = x1 + (x1 - x) * projectedLength;
        float projy2 = y2 + (y2 - y) * projectedLength;

        GL.Begin(GL.TRIANGLES);
        GL.Color(Color.black);

        GL.Vertex(new Vector3(x1, y1, 0));
        GL.Vertex(new Vector3(x2, y1, 0));
        GL.Vertex(new Vector3(projx1, projy1, 0));

        GL.Vertex(new Vector3(x1, y1, 0));
        GL.Vertex(new Vector3(projx2, projy1, 0));
        GL.Vertex(new Vector3(projx1, projy1, 0));

        GL.Vertex(new Vector3(x1, y1, 0));
        GL.Vertex(new Vector3(x1, y2, 0));
        GL.Vertex(new Vector3(projx2, projy2, 0));

        GL.Vertex(new Vector3(x1, y1, 0));
        GL.Vertex(new Vector3(projx2, projy1, 0));
        GL.Vertex(new Vector3(projx2, projy2, 0));

        GL.End();
    }
}
