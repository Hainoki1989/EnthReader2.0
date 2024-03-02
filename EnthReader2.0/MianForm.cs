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
using Newtonsoft.Json.Linq;
using static EnthParser.Models;



namespace EnthReader2._0
{
    public partial class MianForm : Form
    {
        string selectedFileName;
        //EnthParser.EnthParser FileParser;
        EnthParser2 enthParser2;

        public MianForm()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //FileParser = new EnthParser.EnthParser();
            //t_hexDisplay.Font = new Font("Courier New", 10, FontStyle.Regular);
            //t_hexDisplay.ScrollBars = ScrollBars.Vertical;
        }


        private void PopulateTreeView(JObject json, TreeNodeCollection nodes)
        {
            foreach (var property in json.Properties())
            {
                TreeNode node = nodes.Add(property.Name);

                if (property.Value.Type == JTokenType.Object)
                {
                    PopulateTreeView((JObject)property.Value, node.Nodes);
                }
                else if (property.Value.Type == JTokenType.Array)
                {
                    PopulateTreeView((JArray)property.Value, node.Nodes);
                }
                else
                {
                    node.Nodes.Add(property.Value.ToString());
                }
            }
        }

        private void PopulateTreeView(JArray jsonArray, TreeNodeCollection nodes)
        {
            for (int i = 0; i < jsonArray.Count; i++)
            {
                TreeNode node = nodes.Add($"[{i}]");

                if (jsonArray[i].Type == JTokenType.Object)
                {
                    PopulateTreeView((JObject)jsonArray[i], node.Nodes);
                }
                else if (jsonArray[i].Type == JTokenType.Array)
                {
                    PopulateTreeView((JArray)jsonArray[i], node.Nodes);
                }
                else
                {
                    node.Nodes.Add(jsonArray[i].ToString());
                }
            }
        }


        private void b_LoadFile_Click(object sender, EventArgs e)
        {


            enthParser2 = new EnthParser2();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CAR Files (*.car)|*.car|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectedFileName = openFileDialog.FileName;

                enthParser2.LoadFile(selectedFileName);

                Loading loadingForm = new Loading() ;

                Task.Run(() =>
                {
                    // Create an instance of the form
                    loadingForm = new Loading();

                    // Show the form
                    Application.Run(loadingForm);
                });

                //t_hexDisplay.Text = json;

                DisplayFile();


                loadingForm.Invoke((MethodInvoker)delegate
                {
                    loadingForm.Close();
                });

            }

            
            

            //t_hexDisplay.Text = stringBuilder.ToString();

            
                
                
        }

        private void DisplayFile()
        {
            string json = JsonConvert.SerializeObject(enthParser2.enthFile, Formatting.Indented);
            JObject jsonObject = JObject.Parse(json);

            PopulateTreeView(jsonObject, t_LODDisplay.Nodes);


            File.WriteAllText("DEBUGOUTPUT.json", json);
        }

        private void b_ExportOBJ_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            // Set properties for the FolderBrowserDialog
            folderBrowserDialog.Description = "Select the folder to save the file";

            string[] split = selectedFileName.Split('\\');
            string[] name = split.Last().Split('.');
            

            // Show the FolderBrowserDialog
            DialogResult result = folderBrowserDialog.ShowDialog();

            // Process the result
            if (result == DialogResult.OK)
            {
                // Get the selected folder path
                string selectedFolderPath = folderBrowserDialog.SelectedPath;

                // Perform actions with the selected folder path (e.g., save data)
                Console.WriteLine("Selected folder: " + selectedFolderPath);

                if(cb_checkInd.Checked)
                {
                    enthParser2.enthFile.ExportIndivdualLODMeshes(selectedFolderPath, name.First());
                }
                else
                {
                    enthParser2.enthFile.ToIndividualLODOBJ(selectedFolderPath, name.First());
                }
                
            }
            else
            {
                Console.WriteLine("Save operation canceled");
            }



            //enthParser2.enthFile.ToObj();



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

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
