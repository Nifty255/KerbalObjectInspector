using UnityEngine;

namespace KerbalObjectInspector
{
    class WireFrame : MonoBehaviour
    {
        public Material lineMaterial;
        public Color lineColor = new Color(0.0f, 1.0f, 0.0f);

        private Mesh toRender;

        void Start()
        {
            if (lineMaterial == null)
            {
                lineMaterial = new Material(Shader.Find("Self-Illumin/Diffuse"));
            }

            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;

            toRender = new Mesh();
        }

        void OnRenderObject()
        {
            MeshFilter filter = GetComponent<MeshFilter>();
            if (filter)
            {
                DoRender(filter.mesh);
                return;
            }

            SkinnedMeshRenderer skinned = GetComponent<SkinnedMeshRenderer>();
            if (skinned)
            {
                skinned.BakeMesh(toRender);

                Vector3[] verts = toRender.vertices;

                for (int i = 0; i < verts.Length; i++)
                {
                    float x = verts[i].x;
                    float y = verts[i].y;
                    float z = verts[i].z;
                    verts[i] = new Vector3(x / transform.lossyScale.x, y / transform.lossyScale.y, z / transform.lossyScale.z);
                }

                toRender.vertices = verts;
                
                DoRender(toRender);
            }
        }

        void DoRender(Mesh mesh)
        {
            GL.wireframe = true;

            lineMaterial.color = lineColor;

            lineMaterial.SetPass(0);

            Graphics.DrawMeshNow(mesh, transform.localToWorldMatrix);

            GL.wireframe = false;
        }
    }
}
