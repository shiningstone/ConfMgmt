using JbConf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace ConfViews.Upgrade
{
    public partial class PFileUpgradeForm : Form
    {
        private static Logger _log = new Logger("PFileUpgradeForm");
        public PFileUpgradeForm()
        {
            InitializeComponent();

            tbPFileTemplate.Text = ProjectConfig.Get("PFileUpgradeForm.tbPFileTemplate");
            tbOrigDir.Text = ProjectConfig.Get("PFileUpgradeForm.tbOrigDir");
            tbNewDir.Text = ProjectConfig.Get("PFileUpgradeForm.tbNewDir");
        }

        private void btnSelectTemplate_Click(object sender, EventArgs e)
        {
            var file = Utils.UI.Help.AskFile();
            if (!string.IsNullOrEmpty(file))
            {
                tbPFileTemplate.Text = file;
            }
        }

        private void btnSelectOrigDir_Click(object sender, EventArgs e)
        {
            var dir = Utils.UI.Help.AskPath();
            if (!string.IsNullOrEmpty(dir))
            {
                tbOrigDir.Text = dir;
            }
        }

        private void btnSelectNewDir_Click(object sender, EventArgs e)
        {
            var dir = Utils.UI.Help.AskPath();
            if (!string.IsNullOrEmpty(dir))
            {
                tbNewDir.Text = dir;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbPFileTemplate.Text) || string.IsNullOrEmpty(tbOrigDir.Text) || string.IsNullOrEmpty(tbNewDir.Text))
            {
                MessageBox.Show($"输入项不能为空");
                return;
            }

            ProjectConfig.Set($"PFileUpgradeForm.tbPFileTemplate", tbPFileTemplate.Text);
            ProjectConfig.Set($"PFileUpgradeForm.tbOrigDir", tbOrigDir.Text);
            ProjectConfig.Set($"PFileUpgradeForm.tbNewDir", tbNewDir.Text);

            var conf = Builder.Xml.Generate(tbPFileTemplate.Text).Clone() as ConfTree;
            var news = conf.AllItems;

            string first = null;
            ConfTree firstconf = null;
            List<string> except = new List<string>();
            List<string> ADDITION = null, MISSING = null;
            var origfiles = DirOp.GetFiles(tbOrigDir.Text, "*.xml");
            foreach (var file in origfiles)
            {
                var oldconf = Builder.Xml.Generate($"{tbOrigDir.Text}/{file}");
                var olds = oldconf.AllItems;

                if (ADDITION == null && MISSING == null)
                {
                    first = file;
                    firstconf = oldconf;

                    MISSING = olds.Select(x => x.Name).Except(news.Select(x => x.Name)).ToList();
                    ADDITION = news.Select(x => x.Name).Except(olds.Select(x => x.Name)).ToList();
                    _log.Debug($"废弃项:{string.Join(",", MISSING)}{Environment.NewLine}新增项:{string.Join(",", ADDITION)}");

                    var form = new PFileUpgradeRuleEditor(MISSING, news.FindAll(x => ADDITION.Contains(x.Name)));
                    form.ShowDialog();

                    try
                    {
                        var reserves = MISSING.Except(form.Missings).ToList();
                        if (reserves.Count > 0)
                        {
                            _log.Debug($"保留项:{string.Join(",", reserves)}");
                            foreach (var r in reserves)
                            {
                                var node = olds.Find(x => x.Name == r);
                                var parent = conf.Find(node.Parent.Name) as ConfTree;
                                parent.AddNode(node.Clone());
                            }
                        }
                        MISSING = form.Missings;

                        foreach (var item in form.Additions)
                        {
                            conf[item.Name] = item.Value;
                        }
                    }
                    catch (Exception ex)
                    {
                        _log.Error($"升级规则设置失败", ex);
                    }
                }
                else
                {
                    var miss = firstconf.AllItems.Select(x => x.Name).Except(oldconf.AllItems.Select(x => x.Name));
                    var add = oldconf.AllItems.Select(x => x.Name).Except(firstconf.AllItems.Select(x => x.Name));

                    if (miss.Count() > 0 || add.Count() > 0)
                    {
                        except.Add(file);
                        _log.Warn($"{file}与{first}存在差异，升级取消: {(add.Count() > 0 ? $"多{string.Join(", ", add)}" : "")};" +
                            $"{(miss.Count() > 0 ? $"少{string.Join(", ", miss)}" : "")}");
                        
                        continue;
                    }
                }

                oldconf.OverWrite(conf);

                var target = $@"{tbNewDir.Text}/{Path.GetFileName(file)}";
                conf.Save(target);

                _log.Info($"{file}升级完成");
            }

            if (except.Count > 0)
            {
                MessageBox.Show($"以下文件配置项与{first}存在差异，升级被取消{Environment.NewLine}" +
                    $"{string.Join(Environment.NewLine, except)}");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
