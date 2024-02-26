using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EnthParser;



namespace EnthReader2._0
{
    public partial class Form1 : Form
    {
        string selectedFileName;
        EnthParser.EnthParser FileParser;

        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FileParser = new EnthParser.EnthParser();
            t_hexDisplay.Font = new Font("Courier New", 10, FontStyle.Regular);
            t_hexDisplay.ScrollBars = ScrollBars.Vertical;
        }

        private void b_LoadFile_Click(object sender, EventArgs e)
        {
            FileParser = new EnthParser.EnthParser();
            t_LODDisplay.Nodes.Clear();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CAR Files (*.car)|*.car|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Get the selected file name
                selectedFileName = openFileDialog.FileName;
                FileParser.LoadModelFile(selectedFileName);

                for (int i = 0; i < FileParser.LoadedFile.VertexBlocks.Count; i++)
                {
                   c_MeshBox.Items.Add(i);
                }

                for(int i=0; i<FileParser.LoadedFile.LODAddresses.Count; i++)
                {
                    TreeNode parentNode = new TreeNode($"Group {i + 1}");

                    foreach (int address in FileParser.LoadedFile.LODAddresses[i])
                    {
                        TreeNode childNode = new TreeNode($"0x{address.ToString("X")}");

                        parentNode.Nodes.Add(childNode);
                    }

                    t_LODDisplay.Nodes.Add(parentNode);
                }

            }
        }

        private void b_ExportOBJ_Click(object sender, EventArgs e)
        {
            List<LOD> lods = new List<LOD>();

            for(int i=0; i<FileParser.LoadedFile.LODAddresses.Count; i++)
            {
                int NextListStartAddress = (i == FileParser.LoadedFile.LODAddresses.Count-1) ? 0x999999 : FileParser.LoadedFile.LODAddresses[(i + 1)][0];

                LOD lod = new LOD();
                lod.meshes = new List<Mesh>();

                for(int j=0; j < FileParser.LoadedFile.LODAddresses[i].Count; j++)
                {
                    Mesh mesh = new Mesh();
                    mesh.AddressesUsed = new List<string>();
                    mesh.Points = new List<System.Numerics.Vector3>();

                    int startAddress = FileParser.LoadedFile.LODAddresses[i][j];
                    int endAddress = (j == FileParser.LoadedFile.LODAddresses[i].Count - 1) ? NextListStartAddress : FileParser.LoadedFile.LODAddresses[i][j + 1];

                    var matchingGroups = FileParser.LoadedFile.VertexBlocks.Where(vertex => vertex.STARTADDRESSFORTHIS >= startAddress && vertex.STARTADDRESSFORTHIS < endAddress);

                    try
                    {
                        foreach(var group in matchingGroups)
                        {

                            mesh.AddressesUsed.Add($"0x{group.STARTADDRESSFORTHIS.ToString("X")}");

                            foreach (var vertexG in group.VertexDataList)
                                mesh.Points.AddRange(vertexG.VertexList);
                        }
                    }
                    catch(Exception ex) 
                    { 
                    }

                    lod.meshes.Add(mesh);

                }

                lods.Add(lod);
            }


            for (int i = 0; i < lods.Count; i++)
            {
                using (StreamWriter writer = new StreamWriter($"{selectedFileName}{i}.obj"))
                {
                    for (int j = 0; j < lods[i].meshes.Count; j++)
                    {
                        //writer.WriteLine($"o Mesh{j}");

                        foreach (var point in lods[i].meshes[j].Points)
                        {
                            writer.WriteLine($"v {point.X} {point.Y} {point.Z}");
                        }

                        writer.WriteLine("");


                    }
                }
            }

        }

        private void t_LODDisplay_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selectedNode = e.Node;

            HandleSelectedNode(selectedNode);
        }

        private void HandleSelectedNode(TreeNode selectedNode)
        {
            try
            {
                int Saddress = int.Parse(selectedNode.Text.Replace("0x",""), NumberStyles.HexNumber);

                TreeNode nextNode = selectedNode.NextNode;

                if(nextNode == null) 
                {
                    TreeNode parent = selectedNode.Parent;
                    
                    if(parent != null)
                    {
                        nextNode = parent.NextNode.FirstNode;
                    }
                
                }


                int Eaddress = int.Parse(nextNode.Text.Replace("0x", ""), NumberStyles.HexNumber);

                var matchingGroups = FileParser.LoadedFile.VertexBlocks.Where(vertex => vertex.STARTADDRESSFORTHIS >= Saddress && vertex.STARTADDRESSFORTHIS < Eaddress);

                StringBuilder stringBuilder = new StringBuilder();

                try
                {
                    foreach (var group in matchingGroups)
                    {
                        foreach (var vertexG in group.VertexDataList)
                        {
                            foreach(var vt in vertexG.VertexList)
                            {
                                stringBuilder.AppendLine($"vertex {vt.X} {vt.Y} {vt.Z}");
                            }
                        }
                    }


                    t_hexDisplay.Text = stringBuilder.ToString();

                }
                catch (Exception ex)
                {
                }


            }
            catch
            {

            }

            Console.WriteLine();
        }
    }
}
