using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Management.Automation;
using System.Windows.Forms;

namespace WindowsDefenderEasyExceptionAdder
{
    public partial class Form1 : Form
    {
        private string currentPath { get; set; }
        private List<string> currentExclusions { get; set; }
        private string selectedItem { get; set; }

        public Form1()
        {
            InitializeComponent();
            GetCurrentExclusions();
        }

        private void GetCurrentExclusions()
        {
            currentExclusions = new List<string>();
            using (PowerShell ps = PowerShell.Create())
            {
                ps.AddScript($"Get-MpPreference");
                var result = ps.Invoke();
                var exclusions = result.FirstOrDefault()?.Members.Where(x => x.Name == "ExclusionPath").Select(x => x.Value);
                for (int i = 0; i < exclusions.Count(); i++)
                {
                    var cur = (exclusions.FirstOrDefault());
                    if (cur != null)
                    {
                        foreach (var path in (cur as string[]))
                        {
                            this.currentExclusions.Add(path);
                        }
                    }
                    this.listBoxCurrentExclusions.DataSource = null;
                    this.listBoxCurrentExclusions.DataSource = this.currentExclusions;
                }
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.ShowDialog();
                this.currentPath = dialog.FileName;
                this.tbFilePath.Text = this.currentPath;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(this.currentPath != string.Empty)
            {
                using (PowerShell ps = PowerShell.Create())
                {
                    ps.AddScript($"Add-MpPreference -ExclusionPath \"{this.currentPath}\"");
                    var result = ps.Invoke();
                }
            }
            GetCurrentExclusions();
            this.tbFilePath.Text = "";
            this.currentPath = "";
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            this.tbFilePath.Text = ((string[])e.Data.GetData(DataFormats.FileDrop, false))[0];
            this.currentPath = this.tbFilePath.Text;
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void listBoxCurrentExclusions_SelectedIndexChanged(object sender, EventArgs e)
        {
            var lb = sender as ListBox;
            this.selectedItem = lb.SelectedItem?.ToString();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (PowerShell ps = PowerShell.Create())
            {
                ps.AddScript($"Remove-MpPreference -ExclusionPath \"{this.selectedItem}\"");
                var result = ps.Invoke();
            }
            GetCurrentExclusions();
        }

        private void btnFolder_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.ShowDialog();
                this.currentPath = dialog.SelectedPath;
                this.tbFilePath.Text = this.currentPath;
            }
        }
    }
}
