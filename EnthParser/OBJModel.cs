using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EnthParser
{
    public class OBJModel
    {
        public string ModelName;
        public List<ModelLOD> modelLods;

        public OBJModel() 
        {
            modelLods = new List<ModelLOD>() ;
        }
    }

    public class ModelLOD //each ofthe LODS in the model normally 0 to 4
    {
        public List<ModelMesh> Meshes;

        public ModelLOD()
        {
            Meshes = new List<ModelMesh>();
        }
    }

    public class ModelMesh // each of the mesh groups 9,10,9,5
    {
        public List<ModelSubMesh> SubMeshes;

        public ModelMesh()
        {
            SubMeshes = new List<ModelSubMesh>();
        }
    }

    public class ModelSubMesh // each of the submeshes in the mesh
    {
        public List<Vector3> MeshVerticies;
        public List<Tri> MeshIndicies;

        public ModelSubMesh()
        {
            MeshVerticies = new List<Vector3>();
            MeshIndicies = new List<Tri>();

        }

    }

    public class Tri
    {
        public int point1;
        public int point2;
        public int point3;
    }
}
