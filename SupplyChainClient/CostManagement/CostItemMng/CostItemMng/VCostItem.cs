using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Linq;

using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.Template;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using Application.Business.Erp.ResourceManager.Client.Main;
using IFramework = VirtualMachine.Component.WinMVC.generic.IFramework;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonForm;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using Application.Business.Erp.SupplyChain.Client.Main;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemCategoryMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemMng
{
    public partial class VCostItem : TBasicDataView
    {
        private CostItemCategory oprNode = null;
        private CostItem optItem = null;
        private bool isNew = true;
        //��Ȩ�޵�ҵ����֯
        private IList lstInstance;
        //Ψһ����
        private string uniqueCode;

        private Hashtable hashtableRules = new Hashtable();

        private Dictionary<string, TreeNode> listCheckedNode = new Dictionary<string, TreeNode>();
        private List<TreeNode> listCopyNode = new List<TreeNode>();

        public MCostItem model;

        public VCostItem(MCostItem mot)
        {
            model = mot;
            InitializeComponent();

            InitForm();
        }

        private void InitForm()
        {
            try
            {
                this.TopMost = true;
                FlashScreen.Show("���ڳ�ʼ������,���Ժ�......");

                InitEvents();

                LoadCostItemCategoryTree();

                foreach (string value in Enum.GetNames(typeof(CostItemContentType)))
                {
                    cbContentType.Items.Add(value);
                }
                cbContentType.SelectedIndex = 0;

                foreach (string value in Enum.GetNames(typeof(CostItemApplyLeve)))
                {
                    cbAdaptLevel.Items.Add(value);
                }
                cbAdaptLevel.SelectedIndex = 0;

                //����ģʽ
                IList list = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetBasicDataByName(VBasicDataOptr.CostItem_ManagementMode);
                if (list != null)
                {
                    foreach (BasicDataOptr bdo in list)
                    {
                        cbManagementMode.Items.Add(bdo);
                    }
                    cbManagementMode.DisplayMember = "BasicName";
                    cbManagementMode.ValueMember = "Id";

                    cbManagementMode.SelectedIndex = 0;
                }

                txtNoDtlColorFlag.BackColor = ColorTranslator.FromHtml("#D7E8FE");

                this.TopMost = true;
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                MessageBox.Show("��ʼ��ʧ�ܣ���ϸ��Ϣ��" + ExceptionUtil.ExceptionMessage(ex));
            }
            finally
            {
                FlashScreen.Close();
                this.TopMost = true;
            }
        }

        private void InitEvents()
        {
            //tvwCategory.NodeMouseClick += new TreeNodeMouseClickEventHandler(tvwCategory_NodeMouseClick);

            //tvwCategory.AfterCheck += new TreeViewEventHandler(tvwCategory_AfterCheck);
            tvwCategory.BeforeSelect += new TreeViewCancelEventHandler(tvwCategory_BeforeSelect);
            tvwCategory.AfterSelect += new TreeViewEventHandler(tvwCategory_AfterSelect);

            //mnuTree.ItemClicked += new ToolStripItemClickedEventHandler(mnuTree_ItemClicked);

            //tvwCategory.ItemDrag += new ItemDragEventHandler(tvwCategory_ItemDrag);
            //tvwCategory.DragEnter += new DragEventHandler(tvwCategory_DragEnter);
            //tvwCategory.DragDrop += new DragEventHandler(tvwCategory_DragDrop);
            //tvwCategory.DragOver += new DragEventHandler(tvwCategory_DragOver);

            tvwCategory.AfterExpand += new TreeViewEventHandler(tvwCategory_AfterExpand);
            btnRefreshTreeStatus.Click += new EventHandler(btnRefreshTreeStatus_Click);

            gridCostItem.CellClick += new DataGridViewCellEventHandler(gridCostItem_CellClick);

            #region �ɱ��������Ϣ����
            btnAddCostItem.Click += new EventHandler(btnAddCostItem_Click);
            btnUpdateCostItem.Click += new EventHandler(btnUpdateCostItem_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnSaveCostItem.Click += new EventHandler(btnSaveCostItem_Click);
            btnPublishCostItem.Click += new EventHandler(btnPublishCostItem_Click);
            btnCancellationCostItem.Click += new EventHandler(btnCancellationCostItem_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);

            cbContentType.SelectedIndexChanged += new EventHandler(cbContentType_SelectedIndexChanged);

            txtProjectUnit.LostFocus += new EventHandler(txtProjectUnit_LostFocus);
            txtPriceUnit.LostFocus += new EventHandler(txtPriceUnit_LostFocus);
            btnSelProjectQnyUnit.Click += new EventHandler(btnSelProjectQnyUnit_Click);
            btnSelPriceUnit.Click += new EventHandler(btnSelPriceUnit_Click);

            txtCostItemCateFilter.LostFocus += new EventHandler(txtCostItemCateFilter_LostFocus);
            txtCostItemCateFilter2.LostFocus += new EventHandler(txtCostItemCateFilter_LostFocus);
            #endregion

            #region ��Դ���ö���
            //��̬���÷ֿ�Ŀ�ɱ��Ͱ�ť��anchor
            gridCostSubject.Width = 608;
            gridCostSubject.Height = 186;
            gridCostSubject.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            btnAddCostSubject.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnUpdateCostSubject.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDeleteCostSubject.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPublishCostSubject.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCancellationCostSubject.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnFreezeCostSubject.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            btnAddCostSubject.Click += new EventHandler(btnAddCostSubject_Click);
            btnUpdateCostSubject.Click += new EventHandler(btnUpdateCostSubject_Click);
            btnDeleteCostSubject.Click += new EventHandler(btnDeleteCostSubject_Click);
            btnPublishCostSubject.Click += new EventHandler(btnPublishCostSubject_Click);
            btnFreezeCostSubject.Click += new EventHandler(btnFreezeCostSubject_Click);
            btnCancellationCostSubject.Click += new EventHandler(btnCancellationCostSubject_Click);
            #endregion

            tabBaseInfo.Selected += new TabControlEventHandler(tabBaseInfo_Selected);

            btnSearch.Click += new EventHandler(btnSearch_Click);

            btnCateFilter1.Click += new EventHandler(btnCateFilter1_Click);
            btnCateFilter2.Click += new EventHandler(btnCateFilter2_Click);

            btnSubjectFilter1.Click += new EventHandler(btnSubjectFilter1_Click);
            btnSubjectFilter2.Click += new EventHandler(btnSubjectFilter2_Click);
            btnSubjectFilter3.Click += new EventHandler(btnSubjectFilter3_Click);

            txtCostSubjectFilter1.LostFocus += new EventHandler(txtCostSubjectFilter1_LostFocus);
            txtCostSubjectFilter2.LostFocus += new EventHandler(txtCostSubjectFilter1_LostFocus);
            txtCostSubjectFilter3.LostFocus += new EventHandler(txtCostSubjectFilter1_LostFocus);

            #region �Ͷ�������
            InitWFEvents();
            #endregion
        }


        //ѡ��������1
        void btnCateFilter1_Click(object sender, EventArgs e)
        {
            VSelectCostItemCategory frm = new VSelectCostItemCategory();
            frm.ShowDialog();

            if (frm.IsOK)
            {
                txtCostItemCateFilter.Tag = frm.SelectCategory;
                txtCostItemCateFilter.Text = frm.SelectCategory.Name;
            }
        }
        //ѡ��������2
        void btnCateFilter2_Click(object sender, EventArgs e)
        {
            VSelectCostItemCategory frm = new VSelectCostItemCategory();
            frm.ShowDialog();

            if (frm.IsOK)
            {
                txtCostItemCateFilter2.Tag = frm.SelectCategory;
                txtCostItemCateFilter2.Text = frm.SelectCategory.Name;
            }
        }
        //ɾ���������
        void txtCostItemCateFilter_LostFocus(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.Text.Trim() == "")
            {
                tb.Tag = null;
            }
        }

        //��Ŀ����1
        void btnSubjectFilter1_Click(object sender, EventArgs e)
        {
            VSelectCostAccountSubject frm = new VSelectCostAccountSubject(new MCostAccountSubject());
            frm.ShowDialog();

            if (frm.isOK)
            {
                txtCostSubjectFilter1.Tag = frm.SelectAccountSubject;
                txtCostSubjectFilter1.Text = frm.SelectAccountSubject.Name;
            }
        }
        //��Ŀ����2
        void btnSubjectFilter2_Click(object sender, EventArgs e)
        {
            VSelectCostAccountSubject frm = new VSelectCostAccountSubject(new MCostAccountSubject());
            frm.ShowDialog();

            if (frm.isOK)
            {
                txtCostSubjectFilter2.Tag = frm.SelectAccountSubject;
                txtCostSubjectFilter2.Text = frm.SelectAccountSubject.Name;
            }
        }
        //��Ŀ����3
        void btnSubjectFilter3_Click(object sender, EventArgs e)
        {
            VSelectCostAccountSubject frm = new VSelectCostAccountSubject(new MCostAccountSubject());
            frm.ShowDialog();

            if (frm.isOK)
            {
                txtCostSubjectFilter3.Tag = frm.SelectAccountSubject;
                txtCostSubjectFilter3.Text = frm.SelectAccountSubject.Name;
            }
        }
        //ɾ����Ŀ����
        void txtCostSubjectFilter1_LostFocus(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.Text.Trim() == "")
            {
                tb.Tag = null;
            }
        }

        void txtProjectUnit_LostFocus(object sender, EventArgs e)
        {
            txtPriceUnit.LostFocus -= new EventHandler(txtPriceUnit_LostFocus);
            SetStandardUnit(sender);
            txtPriceUnit.LostFocus += new EventHandler(txtPriceUnit_LostFocus);
        }
        void txtPriceUnit_LostFocus(object sender, EventArgs e)
        {
            txtProjectUnit.LostFocus -= new EventHandler(txtProjectUnit_LostFocus);
            SetStandardUnit(sender);
            txtProjectUnit.LostFocus += new EventHandler(txtProjectUnit_LostFocus);
        }

        private void SetStandardUnit(object sender)
        {
            TextBox tbUnit = sender as TextBox;
            string name = tbUnit.Text.Trim();
            if (name != "")
            {
                if (tbUnit.Tag == null || (tbUnit.Tag != null && (tbUnit.Tag as StandardUnit).Name != name))
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Name", name));
                    IList list = model.ObjectQuery(typeof(StandardUnit), oq);
                    if (list.Count > 0)
                    {
                        tbUnit.Tag = list[0] as StandardUnit;
                    }
                    else
                    {
                        MessageBox.Show("ϵͳĿǰ�����ڸü�����λ�����飡");
                        SelectUnit(tbUnit);
                    }
                }
            }
            else
                tbUnit.Tag = null;
        }

        void btnSelPriceUnit_Click(object sender, EventArgs e)
        {
            SelectUnit(txtPriceUnit);
        }

        void btnSelProjectQnyUnit_Click(object sender, EventArgs e)
        {
            SelectUnit(txtProjectUnit);
        }

        private void SelectUnit(TextBox txt)
        {
            StandardUnit su = UCL.Locate("������λά��", StandardUnitExcuteType.OpenSelect) as StandardUnit;
            if (su != null)
            {
                txt.Tag = su;
                txt.Text = su.Name;
                txt.Focus();
            }
        }


        #region �ɱ������
        //�ɱ����е��
        void gridCostItem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                CostItem tempItem = gridCostItem.Rows[e.RowIndex].Tag as CostItem;
                if (optItem != null && tempItem.Id == optItem.Id)
                    return;

                if (btnSaveCostItem.Enabled && optItem != null && tempItem.Id != optItem.Id)
                {
                    if (MessageBox.Show("����δ����ĳɱ�����Ϣ��Ҫ������", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (!SaveCostItem())
                        {
                            //�޸�ʱ��ԭѡ�е��޸���
                            foreach (DataGridViewRow row in gridCostItem.Rows)
                            {
                                CostItem temp = row.Tag as CostItem;
                                if (temp.Id == optItem.Id)
                                {
                                    gridCostItem.CurrentCell = row.Cells[0];
                                    break;
                                }
                            }
                            return;
                        }
                    }
                }

                optItem = gridCostItem.Rows[e.RowIndex].Tag as CostItem;
                GetCostItemDetail();
                //gridCostItem.CurrentCell = gridCostItem.Rows[e.RowIndex].Cells[0];
                RefreshDetailControls(MainViewState.Browser);
            }
        }
        void tabBaseInfo_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage.Text == "�ֿ�Ŀ�ɱ���Ϣ")
            {

            }
        }
        void btnAddCostItem_Click(object sender, EventArgs e)
        {
            if (oprNode == null)
            {
                MessageBox.Show("����ѡ��ɱ������ڵ㣡");
                tvwCategory.Focus();
                return;
            }
            //�ɱ����У��
            if (btnSaveCostItem.Enabled && optItem != null)
            {
                if (MessageBox.Show("����δ����ĳɱ�����Ϣ��Ҫ������", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!SaveCostItem())
                        return;
                }
            }

            ClearCostItemAll();

            RefreshDetailControls(MainViewState.Modify);

            AddAndSetNewCostItemInfo();

            tabBaseInfo.SelectedIndex = 0;

            txtCostItemName.Focus();
        }
        void btnUpdateCostItem_Click(object sender, EventArgs e)
        {
            if (gridCostItem.SelectedRows.Count == 0)
            {
                MessageBox.Show("��ѡ��Ҫ�޸ĵ��У�");
                return;
            }
            if (gridCostItem.SelectedRows.Count > 1)
            {
                MessageBox.Show("һ��ֻ���޸�һ�У�");
                return;
            }

            CostItem tempItem = gridCostItem.SelectedRows[0].Tag as CostItem;
            if (tempItem.ItemState == CostItemState.����)
            {
                MessageBox.Show("ѡ���в������޸ģ���ѡ��״̬Ϊ���ƶ����򡰷������ĳɱ��");
                return;
            }
            optItem = tempItem;

            GetCostItemDetail();

            RefreshDetailControls(MainViewState.Modify);
        }
        //����ɱ���
        void btnSave_Click(object sender, EventArgs e)
        {
            SaveCostItem();
        }
        //����ɱ���
        void btnSaveCostItem_Click(object sender, EventArgs e)
        {
            SaveCostItem();
        }
        //����
        void btnCancel_Click(object sender, EventArgs e)
        {
            if (gridCostItem.Rows.Count > 0)
            {
                optItem = gridCostItem.Rows[gridCostItem.CurrentCell.RowIndex].Tag as CostItem;
                GetCostItemDetail();
                RefreshDetailControls(MainViewState.Browser);
            }
        }
        //����
        void btnPublishCostItem_Click(object sender, EventArgs e)
        {
            if (gridCostItem.SelectedRows.Count == 0)
            {
                MessageBox.Show("��ѡ��Ҫ�����ĳɱ��");
                return;
            }

            IList list = new List<CostItem>();

            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            foreach (DataGridViewRow row in gridCostItem.SelectedRows)
            {
                CostItem item = row.Tag as CostItem;
                if (item.ItemState == CostItemState.�ƶ�)
                {
                    dis.Add(Expression.Eq("Id", item.Id));
                }
            }
            oq.AddFetchMode("ListQuotas", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("ListCostWorkForce", NHibernate.FetchMode.Eager);
            oq.AddCriterion(dis);

            list = model.ObjectQuery(typeof(CostItem), oq);

            if (list.Count == 0)
            {
                MessageBox.Show("ѡ������û�з��Ϸ������У���ѡ��״̬Ϊ���ƶ����ĳɱ��");
                return;
            }

            for (int i = 0; i < list.Count; i++)
            {
                CostItem item = list[i] as CostItem;

                for (int j = 0; j < item.ListQuotas.Count; j++)
                {
                    SubjectCostQuota costQuota = item.ListQuotas.ElementAt(j);
                    if (costQuota.State == SubjectCostQuotaState.����)
                        costQuota.State = SubjectCostQuotaState.��Ч;
                }

                item.ItemState = CostItemState.����;
                list[i] = item;
            }

            try
            {
                list = model.SaveOrUpdateCostItem(list);

                foreach (CostItem temp in list)
                {
                    foreach (DataGridViewRow row in gridCostItem.SelectedRows)
                    {
                        CostItem item = row.Tag as CostItem;
                        if (temp.Id == item.Id)
                        {

                            //������ϸ
                            if (gridCostSubject.Rows.Count > 0)
                            {
                                SubjectCostQuota tempQuota = gridCostSubject.Rows[0].Tag as SubjectCostQuota;
                                if (tempQuota.TheCostItem.Id == temp.Id)
                                {
                                    foreach (DataGridViewRow rowQuota in gridCostSubject.Rows)
                                    {
                                        tempQuota = rowQuota.Tag as SubjectCostQuota;
                                        foreach (SubjectCostQuota costQuota in temp.ListQuotas)
                                        {
                                            if (tempQuota.Id == costQuota.Id)
                                            {
                                                rowQuota.Tag = costQuota;
                                                rowQuota.Cells[SubjectState.Name].Value = costQuota.State.ToString();
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            if (optItem == null || (!string.IsNullOrEmpty(optItem.Id) && optItem.Id == temp.Id))
                                optItem = temp;

                            row.Tag = temp;
                            row.Cells[ItemState.Name].Value = temp.ItemState.ToString();
                        }
                    }
                }

                RefreshDetailControls(MainViewState.Browser);

                MessageBox.Show("�����ɹ���");
            }
            catch (Exception ex)
            {
                MessageBox.Show("����ʧ�ܣ��쳣��Ϣ��" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        void btnCancellationCostItem_Click(object sender, EventArgs e)
        {
            if (gridCostItem.SelectedRows.Count == 0)
            {
                MessageBox.Show("��ѡ��Ҫ���ϵ��У�");
                return;
            }
            IList list = new List<CostItem>();
            List<int> listRowIndex = new List<int>();
            foreach (DataGridViewRow row in gridCostItem.SelectedRows)
            {
                CostItem item = row.Tag as CostItem;
                if (item.ItemState == CostItemState.����)
                {
                    CostItem temp = model.GetObjectById(typeof(CostItem), item.Id) as CostItem;
                    temp.ItemState = CostItemState.����;
                    list.Add(temp);

                    listRowIndex.Add(row.Index);
                }
            }
            if (list.Count == 0)
            {
                MessageBox.Show("ѡ������û�з������ϵ��У���ѡ��״̬Ϊ���������ĳɱ��");
                return;
            }

            try
            {
                list = model.SaveOrUpdateCostItem(list);

                for (int i = 0; i < listRowIndex.Count; i++)
                {
                    int rowIndex = listRowIndex[i];

                    CostItem temp = list[i] as CostItem;
                    if (optItem != null && temp.Id == optItem.Id)
                        optItem = temp;
                    gridCostItem.Rows[rowIndex].Tag = temp;
                    gridCostItem.Rows[rowIndex].Cells["ItemState"].Value = temp.ItemState.ToString();
                }

                RefreshDetailControls(MainViewState.Browser);

                MessageBox.Show("���óɹ���");
            }
            catch (Exception ex)
            {
                MessageBox.Show("����ʧ�ܣ��쳣��Ϣ��" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridCostItem.SelectedRows.Count == 0)
                {
                    MessageBox.Show("��ѡ��Ҫɾ�����У�");
                    gridCostItem.Focus();
                    return;
                }

                IList list = new List<CostItem>();
                List<int> listRowIndex = new List<int>();
                foreach (DataGridViewRow row in gridCostItem.SelectedRows)
                {
                    CostItem item = row.Tag as CostItem;
                    if (item.ItemState == CostItemState.�ƶ�)
                    {
                        list.Add(item);
                        listRowIndex.Add(row.Index);
                    }
                }

                if (list.Count == 0)
                {
                    MessageBox.Show("ѡ������û�з���ɾ���ļ�¼��ֻ��ɾ��״̬Ϊ���ƶ����ĳɱ��");
                    return;
                }

                if (MessageBox.Show("ɾ�����ָܻ�����ȷ��Ҫɾ��ѡ����״̬Ϊ���ƶ����ĳɱ�����", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                model.DeleteCostItem(list);

                listRowIndex.Sort();
                for (int i = listRowIndex.Count - 1; i > -1; i--)
                {
                    gridCostItem.Rows.RemoveAt(listRowIndex[i]);
                }

                gridCostItem.ClearSelection();

                ClearCostItemAll();

                RefreshDetailControls(MainViewState.Browser);

            }
            catch (Exception ex)
            {
                MessageBox.Show("ɾ��ʧ�ܣ��쳣��Ϣ��" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        void cbContentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbContentType.SelectedItem != null && cbContentType.SelectedItem.ToString() == "�ְ�ȡ��")
            {
                txtPricingRate.ReadOnly = false;
                txtPricingRate.Focus();
            }
            else
            {
                txtPricingRate.ReadOnly = true;
                txtPricingRate.Text = "";
            }
        }

        private void AddAndSetNewCostItemInfo()
        {
            optItem = new CostItem();

            int code = gridCostItem.Rows.Count + 1;
            //��ȡ�������������ϸ��
            for (int i = 0; i < gridCostItem.Rows.Count; i++)
            {
                CostItem item = gridCostItem.Rows[i].Tag as CostItem;

                if (item != null && !string.IsNullOrEmpty(item.Code))
                {
                    try
                    {
                        if (item.Code.IndexOf("-") > -1)
                        {
                            int tempCode = Convert.ToInt32(item.Code.Substring(item.Code.LastIndexOf("-") + 1));
                            if (tempCode >= code)
                                code = tempCode + 1;
                        }
                    }
                    catch
                    {

                    }
                }
            }
            optItem.Code = oprNode.Code + "-" + code.ToString().PadLeft(5, '0');
            optItem.ItemState = CostItemState.�ƶ�;

            CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
            if (projectInfo != null)
            {
                optItem.TheProjectGUID = projectInfo.Id;
                optItem.TheProjectName = projectInfo.Name;
            }

            optItem.TheCostItemCategory = oprNode;
            optItem.TheCostItemCateSyscode = oprNode.SysCode;

            txtCostItemCode.Text = optItem.Code;
        }
        private void AddCostItemInfoInGrid(CostItem dtl)
        {
            int index = gridCostItem.Rows.Add();
            DataGridViewRow row = gridCostItem.Rows[index];
            row.Cells[ItemName.Name].Value = dtl.Name;
            row.Cells[ItemCode.Name].Value = dtl.Code;
            row.Cells[ItemQuotaCode.Name].Value = dtl.QuotaCode;
            row.Cells[ItemState.Name].Value = dtl.ItemState.ToString();
            row.Cells[ItemContentType.Name].Value = dtl.ContentType.ToString();
            row.Cells[ItemManagementMode.Name].Value = dtl.ManagementModeName;
            row.Cells[ItemDesc.Name].Value = dtl.CostDesc;
            row.Cells[ItemCateCode.Name].Value = dtl.TheCostItemCategory.Code;
            row.Cells[ItemCateName.Name].Value = dtl.TheCostItemCategory.Name;
            row.Tag = dtl;

            gridCostItem.CurrentCell = row.Cells[0];
        }
        private void UpdateCostItemInfoInGrid(CostItem dtl)
        {
            for (int i = 0; i < gridCostItem.Rows.Count; i++)
            {
                DataGridViewRow row = gridCostItem.Rows[i];
                CostItem d = row.Tag as CostItem;
                if (d.Id == dtl.Id)
                {
                    row.Cells[ItemName.Name].Value = dtl.Name;
                    row.Cells[ItemCode.Name].Value = dtl.Code;
                    row.Cells[ItemQuotaCode.Name].Value = dtl.QuotaCode;
                    row.Cells[ItemState.Name].Value = dtl.ItemState.ToString();
                    row.Cells[ItemContentType.Name].Value = dtl.ContentType.ToString();
                    row.Cells[ItemManagementMode.Name].Value = dtl.ManagementModeName;
                    row.Cells[ItemDesc.Name].Value = dtl.CostDesc;
                    row.Tag = dtl;
                    gridCostItem.CurrentCell = row.Cells[0];
                    break;
                }
                //if (i == gridCostItem.Rows.Count - 1)//���������
                //{
                //    AddCostItemInfoInGrid(dtl);
                //}
            }
        }
        private void GetCostItemDetail()
        {
            try
            {
                ClearCostItemAll();

                txtCostItemCode.Text = optItem.Code;
                txtCostItemName.Text = optItem.Name;
                txtItemQuotaCode.Text = optItem.QuotaCode;
                ckbIsCommonlyUsed.Checked = optItem.IsCommonlyUsed;
                txtProjectUnit.Tag = optItem.ProjectUnitGUID;
                txtProjectUnit.Text = optItem.ProjectUnitName;

                txtPriceUnit.Tag = optItem.PriceUnitGUID;
                txtPriceUnit.Text = optItem.PriceUnitName;

                txtPrice.Text = optItem.Price.ToString();

                txtDesc.Text = optItem.CostDesc;

                cbAdaptLevel.SelectedItem = optItem.ApplyLevel.ToString();
                cbContentType.SelectedItem = optItem.ContentType.ToString();
                txtPricingRate.Text = optItem.PricingRate.ToString();

                if (optItem.ManagementMode != null)
                    cbManagementMode.Text = optItem.ManagementModeName;

                txtCostItemCateFilter.Text = optItem.CateFilterName1;
                txtCostItemCateFilter.Tag = optItem.CateFilter1;
                txtCostItemCateFilter2.Text = optItem.CateFilterName2;
                txtCostItemCateFilter2.Tag = optItem.CateFilter2;

                txtCostSubjectFilter1.Text = optItem.SubjectCateFilterName1;
                txtCostSubjectFilter1.Tag = optItem.SubjectCateFilter1;

                txtCostSubjectFilter2.Text = optItem.SubjectCateFilterName2;
                txtCostSubjectFilter2.Tag = optItem.SubjectCateFilter2;

                txtCostSubjectFilter3.Text = optItem.SubjectCateFilterName3;
                txtCostSubjectFilter3.Tag = optItem.SubjectCateFilter3;

                btnSave.Enabled = true;
                btnSaveCostItem.Enabled = true;
                btnCancel.Enabled = true;

                gridCostSubject.Rows.Clear();

                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("TheCostItem.Id", optItem.Id));
                oq.AddFetchMode("ListResources", NHibernate.FetchMode.Eager);
                oq.AddOrder(NHibernate.Criterion.Order.Asc("CreateTime"));
                IList listQuota = model.ObjectQuery(typeof(SubjectCostQuota), oq);
                foreach (SubjectCostQuota quota in listQuota)
                {
                    AddCostQuotaInfoInGrid(quota);
                }

                ObjectQuery oq1 = new ObjectQuery();
                oq1.AddCriterion(Expression.Eq("TheCostItem.Id", optItem.Id));
                oq1.AddOrder(NHibernate.Criterion.Order.Asc("CreateTime"));
                IList listCWF = model.ObjectQuery(typeof(CostWorkForce), oq);
                foreach (CostWorkForce cwf in listCWF)
                {
                    AddCostWorkForceInfoInGrid(cwf);
                }

                //RefreshDetailControls(MainViewState.Browser);
            }
            catch (Exception exp)
            {
                MessageBox.Show("��ʾ������" + ExceptionUtil.ExceptionMessage(exp));
            }
        }
        private void ClearCostItemAll()
        {
            txtCostItemCode.Text = "";
            txtCostItemName.Text = "";
            txtItemQuotaCode.Text = "";
            ckbIsCommonlyUsed.Checked = false;
            txtProjectUnit.Text = "";
            txtProjectUnit.Tag = null;

            txtPriceUnit.Text = "";
            txtPriceUnit.Tag = null;

            txtPrice.Text = "";
            txtDesc.Text = "";

            cbAdaptLevel.SelectedIndex = 0;
            cbContentType.SelectedIndex = 0;
            txtPricingRate.Text = "";

            cbManagementMode.SelectedIndex = 0;
            txtCostItemCateFilter.Text = "";
            txtCostItemCateFilter2.Text = "";
            txtCostSubjectFilter1.Text = "";
            txtCostSubjectFilter2.Text = "";
            txtCostSubjectFilter3.Text = "";

            btnSave.Enabled = true;
            btnSaveCostItem.Enabled = true;
            btnCancel.Enabled = true;
            dgvWorkForce.Rows.Clear();
            gridCostSubject.Rows.Clear();
        }
        private bool ValideCostItemSave()
        {
            try
            {
                if (optItem == null)
                {
                    optItem = new CostItem();
                }
                else if (!string.IsNullOrEmpty(optItem.Id))
                    optItem = model.GetObjectById(typeof(CostItem), optItem.Id) as CostItem;

                if (string.IsNullOrEmpty(optItem.TheProjectGUID) || string.IsNullOrEmpty(optItem.TheProjectName))
                {
                    CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
                    if (projectInfo != null)
                    {
                        optItem.TheProjectGUID = projectInfo.Id;
                        optItem.TheProjectName = projectInfo.Name;
                    }
                }

                if (txtCostItemName.Text.Trim() == "")
                {
                    MessageBox.Show("�ɱ������Ʋ���Ϊ�գ�");
                    tabBaseInfo.SelectedTab = tabPage1;
                    txtCostItemName.Focus();
                    return false;
                }
                if (txtCostItemCode.Text.Trim() == "")
                {
                    MessageBox.Show("�ɱ�����벻��Ϊ�գ�");
                    tabBaseInfo.SelectedTab = tabPage1;
                    txtCostItemCode.Focus();
                    return false;
                }
                //if (txtItemQuotaCode.Text.Trim() == "")
                //{
                //    MessageBox.Show("�����Ų���Ϊ�գ�");
                //    tabBaseInfo.SelectedTab = tabPage1;
                //    txtItemQuotaCode.Focus();
                //    return false;
                //}
                if (txtProjectUnit.Text.Trim() == "" || txtProjectUnit.Tag == null)
                {
                    MessageBox.Show("��ѡ�񹤳̼�����λ��");
                    tabBaseInfo.SelectedTab = tabPage1;
                    txtProjectUnit.Focus();
                    return false;
                }
                if (txtPriceUnit.Text.Trim() == "" || txtPriceUnit.Tag == null)
                {
                    MessageBox.Show("��ѡ��۸������λ��");
                    tabBaseInfo.SelectedTab = tabPage1;
                    txtPriceUnit.Focus();
                    return false;
                }
                //if (txtPrice.Text.Trim() == "")
                //{
                //    MessageBox.Show("���۲���Ϊ�գ�");
                //    tabBaseInfo.SelectedTab = tabPage1;
                //    txtPrice.Focus();
                //    return false;
                //}

                optItem.Name = txtCostItemName.Text.Trim();
                optItem.Code = txtCostItemCode.Text.Trim();
                if (!string.IsNullOrEmpty(optItem.Code))
                    optItem.Code = optItem.Code.ToUpper();

                optItem.QuotaCode = txtItemQuotaCode.Text.Trim();
                if (!string.IsNullOrEmpty(optItem.QuotaCode))
                    optItem.QuotaCode = optItem.QuotaCode.ToUpper();

                optItem.IsCommonlyUsed = ckbIsCommonlyUsed.Checked;
                optItem.ProjectUnitGUID = txtProjectUnit.Tag as StandardUnit;
                if (optItem.ProjectUnitGUID != null)
                    optItem.ProjectUnitName = txtProjectUnit.Text.Trim();

                optItem.PriceUnitGUID = txtPriceUnit.Tag as StandardUnit;
                if (optItem.PriceUnitGUID != null)
                    optItem.PriceUnitName = optItem.PriceUnitGUID.Name;

                try
                {
                    decimal Price = 0;
                    if (txtPrice.Text.Trim() != "")
                        Price = Convert.ToDecimal(txtPrice.Text);

                    optItem.Price = Price;
                }
                catch
                {
                    tabBaseInfo.SelectedIndex = 0;
                    MessageBox.Show("���۸�ʽ��д����ȷ��");
                    tabBaseInfo.SelectedTab = tabPage1;
                    txtPrice.Focus();
                    return false;
                }


                optItem.CostDesc = txtDesc.Text.Trim();
                optItem.Summary = oprNode.Summary + "," + optItem.Name;

                //������Ϣ2
                try
                {
                    decimal PricingRate = 0;
                    if (txtPricingRate.Text.Trim() != "")
                        PricingRate = Convert.ToDecimal(txtPricingRate.Text);

                    optItem.PricingRate = PricingRate;
                }
                catch
                {
                    tabBaseInfo.SelectedIndex = 1;
                    MessageBox.Show("�Ƽ۷��ʸ�ʽ��д����ȷ��");
                    tabBaseInfo.SelectedTab = tabPage2;
                    txtPricingRate.Focus();
                    return false;
                }
                optItem.ManagementMode = cbManagementMode.SelectedItem as BasicDataOptr;

                if (optItem.ManagementMode != null)
                    optItem.ManagementModeName = optItem.ManagementMode.BasicName;

                optItem.ContentType = VirtualMachine.Component.Util.EnumUtil<CostItemContentType>.FromDescription(cbContentType.SelectedItem);
                optItem.ApplyLevel = VirtualMachine.Component.Util.EnumUtil<CostItemApplyLeve>.FromDescription(cbAdaptLevel.SelectedItem);

                CostItemCategory cateFilter1 = txtCostItemCateFilter.Tag as CostItemCategory;
                if (cateFilter1 != null)
                {
                    if (optItem.CateFilter1 == null || optItem.CateFilter1.Id != cateFilter1.Id)
                    {
                        optItem.CateFilter1 = cateFilter1;
                        optItem.CateFilterName1 = optItem.CateFilter1.Name;
                        optItem.CateFilterSysCode1 = optItem.CateFilter1.SysCode;
                    }
                }
                else
                {
                    optItem.CateFilter1 = null;
                    optItem.CateFilterName1 = "";
                    optItem.CateFilterSysCode1 = "";
                }

                CostItemCategory cateFilter2 = txtCostItemCateFilter2.Tag as CostItemCategory;
                if (cateFilter2 != null)
                {
                    if (optItem.CateFilter2 == null || optItem.CateFilter2.Id != cateFilter2.Id)
                    {
                        optItem.CateFilter2 = cateFilter2;
                        optItem.CateFilterName2 = optItem.CateFilter2.Name;
                        optItem.CateFilterSysCode2 = optItem.CateFilter2.SysCode;
                    }
                }
                else
                {
                    optItem.CateFilter2 = null;
                    optItem.CateFilterName2 = "";
                    optItem.CateFilterSysCode2 = "";
                }

                //�ֿ�Ŀ��������
                CostAccountSubject subCate = txtCostSubjectFilter1.Tag as CostAccountSubject;
                if (subCate != null)
                {
                    if (optItem.SubjectCateFilter1 == null || optItem.SubjectCateFilter1.Id != subCate.Id)
                    {
                        optItem.SubjectCateFilter1 = subCate;
                        optItem.SubjectCateFilterName1 = subCate.Name;
                        optItem.SubjectCateFilterSyscode1 = subCate.SysCode;
                    }
                }
                else
                {
                    optItem.SubjectCateFilter1 = null;
                    optItem.SubjectCateFilterName1 = null;
                    optItem.SubjectCateFilterSyscode1 = null;
                }

                subCate = txtCostSubjectFilter2.Tag as CostAccountSubject;
                if (subCate != null)
                {
                    if (optItem.SubjectCateFilter2 == null || optItem.SubjectCateFilter2.Id != subCate.Id)
                    {
                        optItem.SubjectCateFilter2 = subCate;
                        optItem.SubjectCateFilterName2 = subCate.Name;
                        optItem.SubjectCateFilterSyscode2 = subCate.SysCode;
                    }
                }
                else
                {
                    optItem.SubjectCateFilter2 = null;
                    optItem.SubjectCateFilterName2 = null;
                    optItem.SubjectCateFilterSyscode2 = null;
                }

                subCate = txtCostSubjectFilter3.Tag as CostAccountSubject;
                if (subCate != null)
                {
                    if (optItem.SubjectCateFilter3 == null || optItem.SubjectCateFilter3.Id != subCate.Id)
                    {
                        optItem.SubjectCateFilter3 = subCate;
                        optItem.SubjectCateFilterName3 = subCate.Name;
                        optItem.SubjectCateFilterSyscode3 = subCate.SysCode;
                    }
                }
                else
                {
                    optItem.SubjectCateFilter3 = null;
                    optItem.SubjectCateFilterName3 = null;
                    optItem.SubjectCateFilterSyscode3 = null;
                }

                return true;

            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
                return false;
            }
        }
        private bool SaveCostItem()
        {
            try
            {
                if (!ValideCostItemSave())
                    return false;
                if (optItem.Id == null)
                {
                    if (optItem.ItemState == 0)
                        optItem.ItemState = CostItemState.�ƶ�;

                    if (optItem.TheCostItemCategory == null)
                    {
                        optItem.TheCostItemCategory = oprNode;
                        optItem.TheCostItemCateSyscode = oprNode.SysCode;
                    }

                    optItem = model.SaveOrUpdateCostItem(optItem);

                    AddCostItemInfoInGrid(optItem);
                }
                else
                {
                    optItem = model.SaveOrUpdateCostItem(optItem);
                    UpdateCostItemInfoInGrid(optItem);
                }

                RefreshDetailControls(MainViewState.Browser);
            }
            catch (Exception exp)
            {
                MessageBox.Show("����ʧ�ܣ��쳣��Ϣ��" + ExceptionUtil.ExceptionMessage(exp));
                return false;
            }

            return true;
        }
        #endregion

        #region �ֿ�Ŀ�ɱ����������ť
        private void AddCostQuotaInfoInGrid(SubjectCostQuota dtl)
        {
            int index = gridCostSubject.Rows.Add();
            DataGridViewRow row = gridCostSubject.Rows[index];
            row.Cells[SubjectResUsageName.Name].Value = dtl.Name;
            row.Cells[SubjectCateName.Name].Value = dtl.CostAccountSubjectName;

            DataGridViewComboBoxCell cell = row.Cells[SubjectResourceGroup.Name] as DataGridViewComboBoxCell;
            foreach (ResourceGroup rg in dtl.ListResources)
            {
                string matStr = string.IsNullOrEmpty(rg.ResourceTypeQuality) ? "" : rg.ResourceTypeQuality + ".";
                matStr += string.IsNullOrEmpty(rg.ResourceTypeSpec) ? "" : rg.ResourceTypeSpec + ".";
                matStr += rg.ResourceTypeName;
                matStr += string.IsNullOrEmpty(rg.DiagramNumber) ? "" : "." + rg.DiagramNumber;
                cell.Items.Add(matStr);
            }
            if (cell.Items.Count > 0)
                cell.Value = cell.Items[0].ToString();

            row.Cells[SubjectState.Name].Value = dtl.State.ToString();
            row.Cells[SubjectQuotaQuantity.Name].Value = dtl.QuotaProjectAmount.ToString();

            row.Cells[SubjectQuantityPrice.Name].Value = dtl.QuotaPrice.ToString();
            row.Cells[SubjectPriceUnit.Name].Value = dtl.PriceUnitName;

            row.Cells[SubjectQuantityUnit.Name].Value = dtl.ProjectAmountUnitName;
            row.Cells[SubjectWorkAmountPrice.Name].Value = dtl.QuotaMoney.ToString();

            row.Cells[SubjectWastage.Name].Value = dtl.Wastage.ToString();
            row.Cells[SubjectAssessmentRate.Name].Value = dtl.AssessmentRate.ToString();

            row.Cells[SubjectMainResourceFlag.Name].Value = dtl.MainResourceFlag ? "��" : "��";

            row.Tag = dtl;

            gridCostSubject.CurrentCell = row.Cells[0];

            //gridCostSubject.AutoResizeColumns();
        }
        private void UpdateCostQuotaInfoInGrid(SubjectCostQuota dtl)
        {
            foreach (DataGridViewRow row in gridCostSubject.SelectedRows)
            {
                SubjectCostQuota d = row.Tag as SubjectCostQuota;
                if (d.Id == dtl.Id)
                {
                    row.Cells[SubjectResUsageName.Name].Value = dtl.Name;
                    row.Cells[SubjectCateName.Name].Value = dtl.CostAccountSubjectName;

                    DataGridViewComboBoxCell cell = row.Cells[SubjectResourceGroup.Name] as DataGridViewComboBoxCell;
                    cell.Items.Clear();
                    foreach (ResourceGroup rg in dtl.ListResources)
                    {
                        string matStr = string.IsNullOrEmpty(rg.ResourceTypeQuality) ? "" : rg.ResourceTypeQuality + ".";
                        matStr += string.IsNullOrEmpty(rg.ResourceTypeSpec) ? "" : rg.ResourceTypeSpec + ".";
                        matStr += rg.ResourceTypeName;
                        matStr += string.IsNullOrEmpty(rg.DiagramNumber) ? "" : "." + rg.DiagramNumber;
                        cell.Items.Add(matStr);
                    }
                    if (cell.Items.Count > 0)
                        cell.Value = cell.Items[0].ToString();

                    row.Cells[SubjectState.Name].Value = dtl.State.ToString();
                    row.Cells[SubjectQuotaQuantity.Name].Value = dtl.QuotaProjectAmount.ToString();

                    row.Cells[SubjectQuantityPrice.Name].Value = dtl.QuotaPrice.ToString();
                    row.Cells[SubjectPriceUnit.Name].Value = dtl.PriceUnitName;

                    row.Cells[SubjectQuantityUnit.Name].Value = dtl.ProjectAmountUnitName;
                    row.Cells[SubjectWorkAmountPrice.Name].Value = dtl.QuotaMoney.ToString();

                    row.Cells[SubjectWastage.Name].Value = dtl.Wastage.ToString();
                    row.Cells[SubjectAssessmentRate.Name].Value = dtl.AssessmentRate.ToString();

                    row.Cells[SubjectMainResourceFlag.Name].Value = dtl.MainResourceFlag ? "��" : "��";

                    row.Tag = dtl;
                    break;
                }
            }

            //gridCostSubject.AutoResizeColumns();
        }

        void btnAddCostSubject_Click(object sender, EventArgs e)
        {
            if (optItem == null)
            {
                MessageBox.Show("����ѡ��һ���ɱ��");
                return;
            }
            //else if (optItem.ItemState != CostItemState.�ƶ�)
            //{
            //    MessageBox.Show("��ѡ��״̬Ϊ���ƶ����ĳɱ�����в�����");
            //    return;
            //}
            bool addCostItemFlag = false;
            if (string.IsNullOrEmpty(optItem.Id))
                addCostItemFlag = true;

            VEditCostSubjectQuota frm = new VEditCostSubjectQuota(new MCostItem());
            frm.OptCostItem = optItem;
            frm.ShowDialog();

            if (frm.OptCostQuota != null)// && !string.IsNullOrEmpty(frm.OptCostQuota.Id)
            {
                optItem = frm.OptCostItem;
                if (!addCostItemFlag)
                    UpdateCostItemInfoInGrid(optItem);
                AddCostQuotaInfoInGrid(frm.OptCostQuota);
            }
        }
        void btnUpdateCostSubject_Click(object sender, EventArgs e)
        {
            if (gridCostSubject.SelectedRows.Count == 0)
            {
                MessageBox.Show("��ѡ��Ҫ�޸ĵ��У�");
                return;
            }
            if (gridCostSubject.SelectedRows.Count > 1)
            {
                MessageBox.Show("һ��ֻ���޸�һ�У�");
                return;
            }
            SubjectCostQuota quota = gridCostSubject.SelectedRows[0].Tag as SubjectCostQuota;
            if (quota.State == SubjectCostQuotaState.���� || quota.State == SubjectCostQuotaState.����)
            {
                MessageBox.Show("ѡ���в������޸ģ���ѡ��״̬Ϊ�����ơ�����Ч���ķֿ�Ŀ�ɱ���Ϣ��");
                return;
            }
            VEditCostSubjectQuota frm = new VEditCostSubjectQuota(new MCostItem());
            frm.OptCostItem = optItem;
            frm.OptCostQuota = quota;
            frm.ShowDialog();

            UpdateCostQuotaInfoInGrid(frm.OptCostQuota);

        }
        void btnDeleteCostSubject_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridCostSubject.SelectedRows.Count == 0)
                {
                    MessageBox.Show("��ѡ��Ҫɾ�����У�");
                    gridCostSubject.Focus();
                    return;
                }

                IList list = new List<SubjectCostQuota>();
                List<int> listRowIndex = new List<int>();
                foreach (DataGridViewRow row in gridCostSubject.SelectedRows)
                {
                    SubjectCostQuota item = row.Tag as SubjectCostQuota;
                    if (item.State == SubjectCostQuotaState.����)
                    {
                        list.Add(item);
                        listRowIndex.Add(row.Index);
                    }
                }

                if (list.Count == 0)
                {
                    MessageBox.Show("ѡ������û�з���ɾ���ļ�¼��ֻ��ɾ��״̬Ϊ�����ơ��ĳɱ��");
                    return;
                }

                if (MessageBox.Show("ɾ�����ָܻ�����ȷ��Ҫɾ��ѡ����״̬Ϊ�����ơ��ĳɱ�����", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                model.DeleteCostItemQuota(list);

                listRowIndex.Sort();
                for (int i = listRowIndex.Count - 1; i > -1; i--)
                {
                    gridCostSubject.Rows.RemoveAt(listRowIndex[i]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ɾ��ʧ�ܣ��쳣��Ϣ��" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        void btnPublishCostSubject_Click(object sender, EventArgs e)
        {
            if (gridCostSubject.SelectedRows.Count == 0)
            {
                MessageBox.Show("��ѡ��Ҫ�������У�");
                gridCostSubject.Focus();
                return;
            }

            //����У��
            //����Դ��־����ֻ������һ��
            int mainResourceCount = 0;
            #region ��������Դ����   
            //2017-3-15֮ǰ��Ҫ�ж�����ԴΨһ
            //foreach (DataGridViewRow row in gridCostSubject.Rows)
            //{
            //    SubjectCostQuota item = row.Tag as SubjectCostQuota;
            //    if (item.MainResourceFlag && item.State == SubjectCostQuotaState.��Ч)
            //    {
            //        mainResourceCount += 1;
            //    }
            //}
            //foreach (DataGridViewRow row in gridCostSubject.SelectedRows)
            //{
            //    SubjectCostQuota item = row.Tag as SubjectCostQuota;
            //    if (item.MainResourceFlag && (item.State == SubjectCostQuotaState.���� || item.State == SubjectCostQuotaState.����))
            //    {
            //        mainResourceCount += 1;
            //    }
            //}
            //if (mainResourceCount == 0)
            //{
            //    MessageBox.Show("������Դ���ö�����Ϣ�б�������һ������Դ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    gridCostSubject.Focus();
            //    return;
            //}
            //if (mainResourceCount > 1)
            //{
            //    MessageBox.Show("�ɱ�����Դ���ö�����Ϣ�������˶������Դ���˴�ֻ������һ�������飡", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    gridCostSubject.Focus();
            //    return;
            //}
            //2017-3-15����Ҫ�ж�����ԴΨһ
            #endregion
            bool saveFlag = true;

            IList list = new List<SubjectCostQuota>();
            List<int> listRowIndex = new List<int>();
            foreach (DataGridViewRow row in gridCostSubject.SelectedRows)
            {
                SubjectCostQuota item = row.Tag as SubjectCostQuota;
                if (item.State == SubjectCostQuotaState.���� || item.State == SubjectCostQuotaState.����)
                {
                    if (string.IsNullOrEmpty(item.Id))
                    {
                        saveFlag = false;
                        break;
                    }
                    item = model.GetObjectById(typeof(SubjectCostQuota), item.Id) as SubjectCostQuota;
                    item.State = SubjectCostQuotaState.��Ч;
                    list.Add(item);
                    listRowIndex.Add(row.Index);

                }
            }
            if (!saveFlag)
            {
                MessageBox.Show("�����ɱ�����δ���棬���ȱ���ɱ�����Ϣ��");
                return;
            }

            if (list.Count == 0)
            {
                MessageBox.Show("ѡ������û�з��Ϸ����ļ�¼����ѡ��״̬Ϊ�����ơ��򡮶��ᡯ���У�");
                return;
            }


            try
            {

                list = model.SaveOrUpdateCostItemQuota(list);

                for (int i = 0; i < listRowIndex.Count; i++)
                {
                    int rowIndex = listRowIndex[i];
                    gridCostSubject.Rows[rowIndex].Tag = list[i];
                    gridCostSubject.Rows[rowIndex].Cells["SubjectState"].Value = (list[i] as SubjectCostQuota).State.ToString();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("����ʧ�ܣ��쳣��Ϣ��" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        void btnFreezeCostSubject_Click(object sender, EventArgs e)
        {
            if (gridCostSubject.SelectedRows.Count == 0)
            {
                MessageBox.Show("��ѡ��Ҫ������У�");
                gridCostSubject.Focus();
                return;
            }
            IList list = new List<SubjectCostQuota>();
            List<int> listRowIndex = new List<int>();
            foreach (DataGridViewRow row in gridCostSubject.SelectedRows)
            {
                SubjectCostQuota item = row.Tag as SubjectCostQuota;
                if (item.State == SubjectCostQuotaState.��Ч)
                {
                    item = model.GetObjectById(typeof(SubjectCostQuota), item.Id) as SubjectCostQuota;
                    item.State = SubjectCostQuotaState.����;
                    list.Add(item);
                    listRowIndex.Add(row.Index);
                }
            }

            if (list.Count == 0)
            {
                MessageBox.Show("ѡ������û�з��϶���ļ�¼����ѡ��״̬Ϊ����Ч�����У�");
                return;
            }

            try
            {

                list = model.SaveOrUpdateCostItemQuota(list);

                for (int i = 0; i < listRowIndex.Count; i++)
                {
                    int rowIndex = listRowIndex[i];
                    gridCostSubject.Rows[rowIndex].Tag = list[i];
                    gridCostSubject.Rows[rowIndex].Cells["SubjectState"].Value = (list[i] as SubjectCostQuota).State.ToString();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("����ʧ�ܣ��쳣��Ϣ��" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        void btnCancellationCostSubject_Click(object sender, EventArgs e)
        {
            if (gridCostSubject.SelectedRows.Count == 0)
            {
                MessageBox.Show("��ѡ��Ҫ���ϵ��У�");
                gridCostSubject.Focus();
                return;
            }

            IList list = new List<SubjectCostQuota>();
            List<int> listRowIndex = new List<int>();
            foreach (DataGridViewRow row in gridCostSubject.SelectedRows)
            {
                SubjectCostQuota item = row.Tag as SubjectCostQuota;
                if (item.State == SubjectCostQuotaState.��Ч || item.State == SubjectCostQuotaState.����)
                {
                    item = model.GetObjectById(typeof(SubjectCostQuota), item.Id) as SubjectCostQuota;
                    item.State = SubjectCostQuotaState.����;
                    list.Add(item);
                    listRowIndex.Add(row.Index);
                }
            }

            if (list.Count == 0)
            {
                MessageBox.Show("ѡ������û�з������ϵļ�¼����ѡ��״̬Ϊ����Ч���򡮶��ᡯ���У�");
                return;
            }

            try
            {

                list = model.SaveOrUpdateCostItemQuota(list);

                for (int i = 0; i < listRowIndex.Count; i++)
                {
                    int rowIndex = listRowIndex[i];
                    gridCostSubject.Rows[rowIndex].Tag = list[i];
                    gridCostSubject.Rows[rowIndex].Cells["SubjectState"].Value = (list[i] as SubjectCostQuota).State.ToString();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("����ʧ�ܣ��쳣��Ϣ��" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        #endregion

        #region �ɱ���������

        void tvwCategory_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (btnSaveCostItem.Enabled && optItem != null)
            {
                if (MessageBox.Show("����δ����ĳɱ�����Ϣ��Ҫ������", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!SaveCostItem())
                        e.Cancel = true;
                }
            }
        }
        private void tvwCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                oprNode = tvwCategory.SelectedNode.Tag as CostItemCategory;
                this.GetNodeDetail();
                optItem = null;
                ClearCostItemAll();
                RefreshDetailControls(MainViewState.Browser);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
            }
        }

        void tvwCategory_AfterExpand(object sender, TreeViewEventArgs e)
        {
            TreeNode tempNode = new TreeNode();

            string countSql = "select distinct thecostitemcategory from thd_costitem where thecostitemcategory in ( ";
            string where = "";
            foreach (TreeNode tn in e.Node.Nodes)
            {
                where += "'" + tn.Name + "',";
            }
            countSql += where.Substring(0, where.Length - 1) + ")";

            DataSet ds = model.SearchSQL(countSql);
            DataTable dt = ds.Tables[0];

            List<string> listParentId = new List<string>();

            foreach (DataRow row in dt.Rows)
            {
                string parentId = row[0].ToString();
                listParentId.Add(parentId);
            }

            foreach (TreeNode tn in e.Node.Nodes)
            {
                if (listParentId.Contains(tn.Name) == false)
                {
                    tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                    tn.ForeColor = ColorTranslator.FromHtml("#000000");
                }
                else
                {
                    tn.BackColor = tempNode.BackColor;
                    tn.ForeColor = tempNode.ForeColor;
                }
            }
        }

        void btnRefreshTreeStatus_Click(object sender, EventArgs e)
        {
            if (tvwCategory.SelectedNode != null)
            {
                TreeNode tempNode = new TreeNode();

                //��ѯ
                string countSql = "select distinct thecostitemcategory from thd_costitem where thecostitemcategory in ( ";
                string where = "";

                TreeNode selectedNode = tvwCategory.SelectedNode;

                where = "'" + selectedNode.Name + "'";

                if (selectedNode.Nodes.Count > 0)
                {
                    where += ",";

                    foreach (TreeNode tn in selectedNode.Nodes)
                    {
                        where += "'" + tn.Name + "',";
                    }

                    where = where.Substring(0, where.Length - 1);
                }
                countSql += where + ")";

                DataSet ds = model.SearchSQL(countSql);
                DataTable dt = ds.Tables[0];

                //����
                List<string> listParentId = new List<string>();

                foreach (DataRow row in dt.Rows)
                {
                    string parentId = row[0].ToString();
                    listParentId.Add(parentId);
                }

                if (listParentId.Contains(selectedNode.Name) == false)
                {
                    selectedNode.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                    selectedNode.ForeColor = ColorTranslator.FromHtml("#000000");
                }
                else
                {
                    selectedNode.BackColor = tempNode.BackColor;
                    selectedNode.ForeColor = tempNode.ForeColor;
                }

                foreach (TreeNode tn in selectedNode.Nodes)
                {
                    if (listParentId.Contains(tn.Name) == false)
                    {
                        tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                        tn.ForeColor = ColorTranslator.FromHtml("#000000");
                    }
                    else
                    {
                        tn.BackColor = tempNode.BackColor;
                        tn.ForeColor = tempNode.ForeColor;
                    }
                }
            }
        }

        private void GetNodeDetail()
        {
            try
            {
                ClearAll();

                this.txtCurrentPath.Text = tvwCategory.SelectedNode.FullPath;

                GetCostItemByCategory();

                RefreshControls(MainViewState.Browser);
            }
            catch (Exception exp)
            {
                MessageBox.Show("��ʾ������" + ExceptionUtil.ExceptionMessage(exp));
            }
        }

        private void GetCostItemByCategory()
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheCostItemCategory.Id", oprNode.Id));
            oq.AddFetchMode("ProjectUnitGUID", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("TheCostItemCategory", NHibernate.FetchMode.Eager);
            //oq.AddOrder(NHibernate.Criterion.Order.Desc("CreateTime"));
            //oq.AddOrder(NHibernate.Criterion.Order.Desc("UpdateTime"));
            oq.AddOrder(NHibernate.Criterion.Order.Asc("Code"));
            IList list = model.ObjectQuery(typeof(CostItem), oq);

            gridCostItem.Rows.Clear();
            foreach (CostItem item in list)
            {
                AddCostItemInfoInGrid(item);
            }
            gridCostItem.ClearSelection();
        }

        private void ClearAll()
        {
            this.txtCurrentPath.Text = "";
        }

        public void Start()
        {
            RefreshState(MainViewState.Browser);

            LoadCostItemCategoryTree();
        }

        private void LoadCostItemCategoryTree()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwCategory.Nodes.Clear();
                CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
                IList list = model.GetCostItemCategoryByInstance(projectInfo.Id);
                //lstInstance = listAll[1] as IList;
                //IList list = listAll[0] as IList;

                //��ѯȫ���ɱ���
                //ObjectQuery oq = new ObjectQuery();
                //oq.AddFetchMode("TheCostItemCategory", NHibernate.FetchMode.Eager);
                //IList cateList = model.ObjectQuery(typeof(CostItem), oq);

                foreach (CostItemCategory childNode in list)
                {
                    if (childNode.State == 0)
                        continue;

                    TreeNode tnTmp = new TreeNode();
                    tnTmp.Name = childNode.Id.ToString();
                    tnTmp.Text = childNode.Name;
                    tnTmp.Tag = childNode;

                    //bool flag = false;
                    //if (cateList.Count > 0)
                    //{
                    //    foreach (CostItem item in cateList)
                    //    {
                    //        if (childNode.Id == item.TheCostItemCategory.Id)
                    //        {
                    //            flag = true;
                    //            break;
                    //        }
                    //    }
                    //}
                    //if (!flag)
                    //{
                    //    tnTmp.ForeColor = ColorTranslator.FromHtml("#0000ff");
                    //}


                    if (childNode.ParentNode != null)
                    {
                        TreeNode tnp = null;
                        tnp = hashtable[childNode.ParentNode.Id.ToString()] as TreeNode;
                        if (tnp != null)
                        {
                            tnp.Nodes.Add(tnTmp);
                        }
                    }
                    else
                    {
                        tvwCategory.Nodes.Add(tnTmp);
                    }
                    hashtable.Add(tnTmp.Name, tnTmp);
                }
                if (list.Count > 0)
                {
                    this.tvwCategory.SelectedNode = this.tvwCategory.Nodes[0];
                    this.tvwCategory.SelectedNode.Expand();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("���سɱ�����������" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        public void RefreshDetailControls(MainViewState state)
        {
            switch (state)
            {
                case MainViewState.Modify:

                    txtCostItemCode.ReadOnly = false;
                    txtCostItemName.ReadOnly = false;
                    txtItemQuotaCode.ReadOnly = false;
                    ckbIsCommonlyUsed.Enabled = true;
                    txtProjectUnit.ReadOnly = false;
                    txtPriceUnit.ReadOnly = false;

                    btnSelProjectQnyUnit.Enabled = true;
                    btnSelPriceUnit.Enabled = true;

                    txtPrice.ReadOnly = false;
                    txtDesc.ReadOnly = false;

                    cbAdaptLevel.Enabled = true;
                    cbContentType.Enabled = true;

                    if (cbContentType.SelectedItem != null && cbContentType.SelectedItem.ToString() == "�ְ�ȡ��")
                    {
                        txtPricingRate.ReadOnly = false;
                    }

                    cbManagementMode.Enabled = true;

                    txtCostItemCateFilter.ReadOnly = false;
                    btnCateFilter1.Enabled = true;
                    txtCostItemCateFilter2.ReadOnly = false;
                    btnCateFilter2.Enabled = true;

                    txtCostSubjectFilter1.ReadOnly = false;
                    btnSubjectFilter1.Enabled = true;
                    txtCostSubjectFilter2.ReadOnly = false;
                    btnSubjectFilter2.Enabled = true;
                    txtCostSubjectFilter3.ReadOnly = false;
                    btnSubjectFilter3.Enabled = true;

                    btnSave.Enabled = true;
                    btnSaveCostItem.Enabled = true;
                    btnCancel.Enabled = true;

                    //this.btnWFAdd.Enabled 


                    break;

                case MainViewState.Browser:

                    txtCostItemCode.ReadOnly = true;
                    txtCostItemName.ReadOnly = true;
                    txtItemQuotaCode.ReadOnly = true;
                    ckbIsCommonlyUsed.Enabled = false;
                    txtProjectUnit.ReadOnly = true;
                    txtPriceUnit.ReadOnly = true;

                    btnSelProjectQnyUnit.Enabled = false;
                    btnSelPriceUnit.Enabled = false;

                    txtPrice.ReadOnly = true;

                    txtDesc.ReadOnly = true;

                    cbAdaptLevel.Enabled = false;
                    cbContentType.Enabled = false;
                    txtPricingRate.ReadOnly = true;

                    cbManagementMode.Enabled = false;

                    txtCostItemCateFilter.ReadOnly = true;
                    btnCateFilter1.Enabled = false;
                    txtCostItemCateFilter2.ReadOnly = true;
                    btnCateFilter2.Enabled = false;

                    txtCostSubjectFilter1.ReadOnly = true;
                    btnSubjectFilter1.Enabled = false;
                    txtCostSubjectFilter2.ReadOnly = true;
                    btnSubjectFilter2.Enabled = false;
                    txtCostSubjectFilter3.ReadOnly = true;
                    btnSubjectFilter3.Enabled = false;

                    btnSave.Enabled = false;
                    btnSaveCostItem.Enabled = false;
                    btnCancel.Enabled = false;
                    break;
            }
        }

        public override bool ModifyView()
        {
            if (gridCostItem.SelectedRows.Count == 0)
            {
                MessageBox.Show("��ѡ��Ҫ�޸ĵĳɱ��");
                gridCostItem.Focus();
                return false;
            }
            if (gridCostItem.SelectedRows.Count > 1)
            {
                MessageBox.Show("һ��ֻ���޸�һ���ɱ��");
                gridCostItem.Focus();
                return false;
            }

            CostItem tempItem = gridCostItem.SelectedRows[0].Tag as CostItem;
            if (tempItem.ItemState != CostItemState.�ƶ�)
            {
                MessageBox.Show("ѡ��ɱ�������޸ģ���ѡ��״̬Ϊ���ƶ����ĳɱ��");
                return false;
            }
            optItem = tempItem;

            GetCostItemDetail();

            RefreshDetailControls(MainViewState.Modify);

            return true;
        }

        public override bool CancelView()
        {
            try
            {
                GetCostItemDetail();

                RefreshDetailControls(MainViewState.Browser);

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        public override void RefreshView()
        {
            try
            {
                RefreshDetailControls(MainViewState.Browser);

                ClearCostItemAll();

                ClearAll();

                LoadCostItemCategoryTree();
            }
            catch (Exception e)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(e));
            }
        }

        public override bool SaveView()
        {
            return SaveCostItem();
        }

        #endregion

        /// <summary>
        /// �ɱ����ѯ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string cateCode = txtIteamCateCode.Text.Trim();
                string cateName = txtIteamCateName.Text.Trim();
                string quotaCode = txtQuotaCode.Text.Trim();
                string costName = txtCostName.Text.Trim();
                if (cateCode == "" && cateName == "" && quotaCode == "" && costName == "")
                {
                    if (MessageBox.Show("û�в�ѯ�����ή�Ͳ�ѯ�ٶȣ�ȷ��Ҫ������", "ѯ��", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        txtIteamCateCode.Focus();
                        return;
                    }
                }

                FlashScreen.Show("���ڲ�ѯ��������,���Ժ�......");

                ObjectQuery oq = new ObjectQuery();

                CostItemCategory optCateRange = null;

                if (tvwCategory.SelectedNode != null && tvwCategory.SelectedNode.Tag != null)
                {
                    optCateRange = tvwCategory.SelectedNode.Tag as CostItemCategory;
                }

                if (optCateRange != null)
                    oq.AddCriterion(Expression.Like("TheCostItemCateSyscode", optCateRange.SysCode, MatchMode.Start));

                if (quotaCode != "")
                    oq.AddCriterion(Expression.Like("QuotaCode", quotaCode, MatchMode.Anywhere));

                if (costName != "")
                    oq.AddCriterion(Expression.Like("Name", costName, MatchMode.Anywhere));

                if (cateName != "" || cateCode != "")
                {
                    string sql = "select t1.id from thd_costitemcategory t1 where 1=1 ";

                    if (optCateRange != null)
                        sql += " and t1.syscode like '" + optCateRange.SysCode + "%'";

                    if (txtIteamCateCode.Text.Trim() != "")
                        sql += " and t1.code like '%" + cateCode + "%'";
                    if (txtIteamCateName.Text.Trim() != "")
                        sql += " and t1.name like '%" + cateName + "%'";

                    sql += " order by code";

                    DataSet ds = model.SearchSQL(sql);

                    Disjunction dis = new Disjunction();
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            string cateId = row["id"].ToString();
                            dis.Add(Expression.Eq("TheCostItemCategory.Id", cateId));
                        }
                    }
                    oq.AddCriterion(dis);
                }

                oq.AddFetchMode("ProjectUnitGUID", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("TheCostItemCategory", NHibernate.FetchMode.Eager);
                oq.AddOrder(NHibernate.Criterion.Order.Asc("Code"));

                IList list = model.ObjectQuery(typeof(CostItem), oq);

                gridCostItem.Rows.Clear();
                foreach (CostItem item in list)
                {
                    AddCostItemInfoInGrid(item);
                }
                gridCostItem.ClearSelection();

                FlashScreen.Close();
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                MessageBox.Show("��ѯ�������쳣��Ϣ��" + ExceptionUtil.ExceptionMessage(ex));
            }
            finally
            {
                FlashScreen.Close();
            }
        }

        #region �Ͷ�������
        void InitWFEvents()
        {
            this.btnWFAdd.Click += new EventHandler(btnWFAdd_Click);
            this.btnWFUpdate.Click += new EventHandler(btnWFUpdate_Click);
            this.btnWFDelete.Click += new EventHandler(btnWFDelete_Click);
            this.btnWFPublish.Click += new EventHandler(btnWFPublish_Click);
            this.btnWFFreeze.Click += new EventHandler(btnWFFreeze_Click);
            this.btnWFCancellation.Click += new EventHandler(btnWFCancellation_Click);
        }

      
        /// <summary>
        /// �����Ͷ��� ����
        /// </summary>
        void btnWFAdd_Click(object sender, EventArgs e)
        {
            if (optItem == null)
            {
                MessageBox.Show("����ѡ��һ���ɱ��");
                return;
            }
            else if (optItem.ItemState != CostItemState.�ƶ�)
            {
                MessageBox.Show("��ѡ��״̬Ϊ���ƶ����ĳɱ�����в�����");
                return;
            }
            bool addCostItemFlag = false;
            if (string.IsNullOrEmpty(optItem.Id))
                addCostItemFlag = true;

            VEditCostWorkForce frm = new VEditCostWorkForce(new MCostItem());
            frm.OptCostItem = optItem;
            frm.ShowDialog();

            if (frm.OptCostWorkForce != null)// && !string.IsNullOrEmpty(frm.OptCostQuota.Id)
            {
                optItem = frm.OptCostItem;
                if (!addCostItemFlag)
                    UpdateCostItemInfoInGrid(optItem);
                AddCostWorkForceInfoInGrid(frm.OptCostWorkForce);
            }
        }

        private void AddCostWorkForceInfoInGrid(CostWorkForce cwf)
        {
            int index = dgvWorkForce.Rows.Add();
            DataGridViewRow row = dgvWorkForce.Rows[index];
            row.Cells[this.dataGridViewTextBoxColumn1.Name].Value = cwf.Name;
       
            row.Cells[this.dgvtxtcResourceTypeName.Name].Value = cwf.ResourceTypeName;
 
            row.Cells[this.dgvtxtcMinQutity.Name].Value = ClientUtil.ToString(cwf.MinQutity);
            row.Cells[this.dgvtxtcMaxQutity.Name].Value = ClientUtil.ToString(cwf.MaxQutity);
            row.Cells[this.dgvtxtcMaxWorkdays.Name].Value = ClientUtil.ToString(cwf.MaxWorkdays);
            row.Cells[this.dgvtxtcMinWordays.Name].Value = ClientUtil.ToString(cwf.MinWorkdays);

            row.Cells[this.dataGridViewTextBoxColumn4.Name].Value = ClientUtil.ToString(cwf.State);


            row.Tag = cwf;

            dgvWorkForce.CurrentCell = row.Cells[0];
        }
        /// <summary>
        /// �޸��Ͷ�������
        /// </summary>
        void btnWFUpdate_Click(object sender, EventArgs e)
        {
            if (dgvWorkForce.SelectedRows.Count == 0)
            {
                MessageBox.Show("��ѡ��Ҫ�޸ĵ��У�");
                return;
            }
            if (dgvWorkForce.SelectedRows.Count > 1)
            {
                MessageBox.Show("һ��ֻ���޸�һ�У�");
                return;
            }
            CostWorkForce cwf = dgvWorkForce.SelectedRows[0].Tag as CostWorkForce;
            if (cwf.State == CostWorkForceState.���� || cwf.State == CostWorkForceState.����)
            {
                MessageBox.Show("ѡ���в������޸ģ���ѡ��״̬Ϊ�����ơ�����Ч���ķֿ�Ŀ�ɱ���Ϣ��");
                return;
            }
            VEditCostWorkForce frm = new VEditCostWorkForce(new MCostItem());
            frm.OptCostItem = optItem;
            frm.OptCostWorkForce = cwf;
            frm.ShowDialog();

            UpdateCostWorkForceInfoInGrid(frm.OptCostWorkForce);
        }

        private void UpdateCostWorkForceInfoInGrid(CostWorkForce cwf)
        {
            foreach (DataGridViewRow row in dgvWorkForce.SelectedRows)
            {
                CostWorkForce d = row.Tag as CostWorkForce;
                if (d.Id == cwf.Id)
                {
                    row.Cells[this.dataGridViewTextBoxColumn1.Name].Value = cwf.Name;
               
                    row.Cells[this.dgvtxtcResourceTypeName.Name].Value = cwf.ResourceTypeName;

                    row.Cells[this.dgvtxtcMinQutity.Name].Value = ClientUtil.ToString(cwf.MinQutity);
                    row.Cells[this.dgvtxtcMaxQutity.Name].Value = ClientUtil.ToString(cwf.MaxQutity);
                    row.Cells[this.dgvtxtcMaxWorkdays.Name].Value = ClientUtil.ToString(cwf.MaxWorkdays);
                    row.Cells[this.dgvtxtcMinWordays.Name].Value = ClientUtil.ToString(cwf.MinWorkdays);

                    row.Cells[this.dataGridViewTextBoxColumn4.Name].Value = ClientUtil.ToString(cwf.State);

                    row.Tag = cwf;
                    break;
                }
            }
        }

        /// <summary>
        /// ɾ���Ͷ�������
        /// </summary>
        void btnWFDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvWorkForce.SelectedRows.Count == 0)
                {
                    MessageBox.Show("��ѡ��Ҫɾ�����У�");
                    dgvWorkForce.Focus();
                    return;
                }

                IList list = new List<CostWorkForce>();
                List<int> listRowIndex = new List<int>();
                foreach (DataGridViewRow row in dgvWorkForce.SelectedRows)
                {
                    CostWorkForce item = row.Tag as CostWorkForce;
                    if (item.State == CostWorkForceState.����)
                    {
                        list.Add(item);
                        listRowIndex.Add(row.Index);
                    }
                }

                if (list.Count == 0)
                {
                    MessageBox.Show("ѡ������û�з���ɾ���ļ�¼��ֻ��ɾ��״̬Ϊ�����ơ��ĳɱ��");
                    return;
                }

                if (MessageBox.Show("ɾ�����ָܻ�����ȷ��Ҫɾ��ѡ����״̬Ϊ�����ơ��ĳɱ�����", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                model.DeleteCostWorkForce(list);

                listRowIndex.Sort();
                for (int i = listRowIndex.Count - 1; i > -1; i--)
                {
                    dgvWorkForce.Rows.RemoveAt(listRowIndex[i]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ɾ��ʧ�ܣ��쳣��Ϣ��" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        /// <summary>
        /// �����Ͷ�������
        /// </summary>
        void btnWFPublish_Click(object sender, EventArgs e)
        {
            if (dgvWorkForce.SelectedRows.Count == 0)
            {
                MessageBox.Show("��ѡ��Ҫ�������У�");
                dgvWorkForce.Focus();
                return;
            }

            bool saveFlag = true;

            IList list = new List<CostWorkForce>();
            List<int> listRowIndex = new List<int>();
            foreach (DataGridViewRow row in dgvWorkForce.SelectedRows)
            {
                CostWorkForce item = row.Tag as CostWorkForce;
                if (item.State == CostWorkForceState.���� || item.State == CostWorkForceState.����)
                {
                    if (string.IsNullOrEmpty(item.Id))
                    {
                        saveFlag = false;
                        break;
                    }
                    item = model.GetObjectById(typeof(CostWorkForce), item.Id) as CostWorkForce;
                    item.State = CostWorkForceState.��Ч;
                    list.Add(item);
                    listRowIndex.Add(row.Index);

                }
            }
            if (!saveFlag)
            {
                MessageBox.Show("�����ɱ�����δ���棬���ȱ���ɱ�����Ϣ��");
                return;
            }

            if (list.Count == 0)
            {
                MessageBox.Show("ѡ������û�з��Ϸ����ļ�¼����ѡ��״̬Ϊ�����ơ��򡮶��ᡯ���У�");
                return;
            }


            try
            {

                list = model.SaveOrUpdateCostWorkForce(list);

                for (int i = 0; i < listRowIndex.Count; i++)
                {
                    int rowIndex = listRowIndex[i];
                    dgvWorkForce.Rows[rowIndex].Tag = list[i];
                    dgvWorkForce.Rows[rowIndex].Cells[this.dataGridViewTextBoxColumn4.Name].Value = (list[i] as CostWorkForce).State.ToString();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("����ʧ�ܣ��쳣��Ϣ��" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        /// <summary>
        /// �����Ͷ�������
        /// </summary>
        void btnWFFreeze_Click(object sender, EventArgs e)
        {
            if (dgvWorkForce.SelectedRows.Count == 0)
            {
                MessageBox.Show("��ѡ��Ҫ������У�");
                dgvWorkForce.Focus();
                return;
            }
            IList list = new List<CostWorkForce>();
            List<int> listRowIndex = new List<int>();
            foreach (DataGridViewRow row in dgvWorkForce.SelectedRows)
            {
                CostWorkForce item = row.Tag as CostWorkForce;
                if (item.State == CostWorkForceState.��Ч)
                {
                    item = model.GetObjectById(typeof(CostWorkForce), item.Id) as CostWorkForce;
                    item.State = CostWorkForceState.����;
                    list.Add(item);
                    listRowIndex.Add(row.Index);
                }
            }

            if (list.Count == 0)
            {
                MessageBox.Show("ѡ������û�з��϶���ļ�¼����ѡ��״̬Ϊ����Ч�����У�");
                return;
            }

            try
            {

                list = model.SaveOrUpdateCostWorkForce(list);

                for (int i = 0; i < listRowIndex.Count; i++)
                {
                    int rowIndex = listRowIndex[i];
                    dgvWorkForce.Rows[rowIndex].Tag = list[i];
                    dgvWorkForce.Rows[rowIndex].Cells[this.dataGridViewTextBoxColumn4.Name].Value = (list[i] as CostWorkForce).State.ToString();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("����ʧ�ܣ��쳣��Ϣ��" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
            /// <summary>
        /// �����Ͷ�������
        /// </summary>
        void btnWFCancellation_Click(object sender, EventArgs e)
        {
            if (dgvWorkForce.SelectedRows.Count == 0)
            {
                MessageBox.Show("��ѡ��Ҫ���ϵ��У�");
                dgvWorkForce.Focus();
                return;
            }

            IList list = new List<CostWorkForce>();
            List<int> listRowIndex = new List<int>();
            foreach (DataGridViewRow row in dgvWorkForce.SelectedRows)
            {
                CostWorkForce item = row.Tag as CostWorkForce;
                if (item.State == CostWorkForceState.��Ч || item.State == CostWorkForceState.����)
                {
                    item = model.GetObjectById(typeof(CostWorkForce), item.Id) as CostWorkForce;
                    item.State = CostWorkForceState.����;
                    list.Add(item);
                    listRowIndex.Add(row.Index);
                }
            }

            if (list.Count == 0)
            {
                MessageBox.Show("ѡ������û�з������ϵļ�¼����ѡ��״̬Ϊ����Ч���򡮶��ᡯ���У�");
                return;
            }

            try
            {

                list = model.SaveOrUpdateCostWorkForce(list);

                for (int i = 0; i < listRowIndex.Count; i++)
                {
                    int rowIndex = listRowIndex[i];
                    dgvWorkForce.Rows[rowIndex].Tag = list[i];
                    dgvWorkForce.Rows[rowIndex].Cells[this.dataGridViewTextBoxColumn4.Name].Value = (list[i] as CostWorkForce).State.ToString();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("����ʧ�ܣ��쳣��Ϣ��" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        
        
        #endregion
    }
}