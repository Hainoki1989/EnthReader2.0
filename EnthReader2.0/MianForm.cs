using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EnthParser;
using Newtonsoft.Json;



namespace EnthReader2._0
{
    public partial class MianForm : Form
    {
        string selectedFileName;
        //EnthParser.EnthParser FileParser;

        public MianForm()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //FileParser = new EnthParser.EnthParser();
            t_hexDisplay.Font = new Font("Courier New", 10, FontStyle.Regular);
            t_hexDisplay.ScrollBars = ScrollBars.Vertical;
        }

        private void b_LoadFile_Click(object sender, EventArgs e)
        {
            /*FileParser = new EnthParser.EnthParser();
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

            }*/

            EnthParser2 enthParser2 = new EnthParser2();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CAR Files (*.car)|*.car|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectedFileName = openFileDialog.FileName;

                enthParser2.LoadFile(selectedFileName);

            }
        }

        private void b_ExportOBJ_Click(object sender, EventArgs e)
        {

           /* var result = FileParser.LoadedFile.ToOBJ();

            string json = JsonConvert.SerializeObject(result, Formatting.Indented);

            Console.WriteLine("here");

            string OutputFile = $"{DateTime.Now.ToString("ddMMyyyyhhmmss")}";

            if (!Directory.Exists(OutputFile))
                Directory.CreateDirectory(OutputFile);


            string jsonFile = $"{OutputFile}\\{result.ModelName}.json";
            File.WriteAllText(jsonFile, json);

            foreach (var mesh in result.modelLods[0].Meshes) 
            {
                for(int i =0; i<mesh.SubMeshes.Count; i++)
                {
                    int counter = 0;
                    string filename = $"{OutputFile}\\model_{i}.obj";

                    using (StreamWriter writer = new StreamWriter($"{filename}"))
                    {
                        foreach (var vt in mesh.SubMeshes[i].MeshVerticies)
                            writer.WriteLine($"v {vt.X} {vt.Y} {vt.Z}");

                        foreach(var id in mesh.SubMeshes[i].MeshIndicies)
                        {
                            writer.WriteLine($"f {id.point1} {id.point2} {id.point3}");
                        }
                    } 
                }
            }
           */
            
        }

        private void t_LODDisplay_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selectedNode = e.Node;

            HandleSelectedNode(selectedNode);
        }

        private void HandleSelectedNode(TreeNode selectedNode)
        {
           /* try
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
                            stringBuilder.AppendLine("Vertex Group");

                            foreach(var vt in vertexG.VertexList)
                            {
                                stringBuilder.AppendLine($"vertex {vt.X} {vt.Y} {vt.Z}");
                            }
                        }

                        foreach (var index in group.FaceDataList)
                        {
                            stringBuilder.AppendLine("Index Group");

                            foreach(var id in index.faceDataItems)
                            {
                                stringBuilder.Append($" {id.FaceIndex} ");
                                stringBuilder.AppendLine();
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

            Console.WriteLine();*/
        }

        private void b_viewHex_Click(object sender, EventArgs e)
        {
            /*
            HexView hexView = new HexView();
            hexView.DisplayHexData(FileParser.LoadedFile.VertexBlocks[c_MeshBox.SelectedIndex].ReadBytesForDebug.ToArray(), FileParser.LoadedFile.VertexBlocks[c_MeshBox.SelectedIndex].STARTADDRESSFORTHIS);
            hexView.Show();*/
        }
    }
}
