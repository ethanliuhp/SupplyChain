using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Collections;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Service;
using Application.Business.Erp.SupplyChain.Util;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.BasicData.Domain;

namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse
{
    public class MIndicatorUse
    {
        private IDimensionDefineService dimDefSrv;
        private ICubeService cubeSrv;
        private IViewService viewSrv;

        /// <summary>
        /// ��ͼ����
        /// </summary>
        public IViewService ViewSrv
        {
            get 
            {
                if (viewSrv == null)
                {
                    viewSrv = ConstMethod.GetService(typeof(IViewService)) as IViewService;
                }
                return viewSrv; 
            }
            set { viewSrv = value; }
        }

        /// <summary>
        /// ����(����)����
        /// </summary>
        public ICubeService CubeSrv
        {
            get 
            {
                if (cubeSrv == null)
                {
                    cubeSrv = ConstMethod.GetService(typeof(ICubeService)) as ICubeService;
                }
                return cubeSrv; 
            }
            set { cubeSrv = value; }
        }

        /// <summary>
        /// ά�ȶ������
        /// </summary>
        public IDimensionDefineService DimDefSrv
        {
            get 
            {
                if (dimDefSrv == null)
                {
                    dimDefSrv = ConstMethod.GetService(typeof(IDimensionDefineService)) as IDimensionDefineService;
                }
                return dimDefSrv; 
            }
            set { dimDefSrv = value; }
        }

        public MIndicatorUse()
        {
        }

        /// <summary>
        /// ��ά������ת��Ϊ��ά���ʽ
        /// </summary>
        /// <param name="startDimList">�洢��ά��ṹά�ȵļ���</param>
        /// <param name="endDimList">��һά�ȵļ���</param>
        /// <returns></returns>
        private IList Gen2DimData(IList startDimList, IList endDimList)
        {
            IList ret = new ArrayList();

            //���startDimListΪ�գ����endDimListֱ�Ӽӵ�startDimList
            if (startDimList.Count == 0)
            {
                foreach (string dim in endDimList)
                {
                    IList temp = new ArrayList();
                    temp.Add(dim);
                    ret.Add(temp);
                }
            }
            else
            {
                //���startDimList���Ѿ��нṹ����
                foreach (object dimObj in startDimList)
                {
                    foreach (string dim in endDimList)
                    {
                        if (dimObj is IList)
                        {
                            //����Ǽ�����Ѽ����е�ֵ���µ�ά��ֵ������һ���µļ�����
                            IList temp = new ArrayList();
                            IList objList = (IList)dimObj;
                            foreach (string s in objList)
                            {
                                temp.Add(s);
                            }
                            temp.Add(dim);
                            ret.Add(temp);
                        }
                        else
                        {
                            IList temp = new ArrayList();
                            temp.Add(dimObj.ToString());
                            temp.Add(dim);
                            ret.Add(temp);
                        }
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// ��ά�ȴ洢��ʽת���ɶ�ά���ʽ
        /// </summary>
        /// <param name="vStyleList">��ͼ��ʽ����</param>
        /// <param name="displayAttr">��ʾ���� ֵΪId����ʾԭ������Id,������ʾ����</param>
        /// <returns></returns>
        public IList Gen2DimTable(IList vStyleList, string displayAttr)
        {

            IList list = new ArrayList();
            foreach (ViewStyle vs in vStyleList)
            {
                IList valueList = new ArrayList();
                IList indicators = ViewSrv.GetViewStyleDimByVS(vs.Id);
                foreach (ViewStyleDimension vsd in indicators)
                {
                    if (displayAttr.Equals("Id"))
                    {
                        valueList.Add(vsd.DimCatId.ToString());
                    }
                    else
                    {
                        valueList.Add(vsd.Name);
                    }
                }
                list.Add(valueList);
            }

            IList ret = new ArrayList();

            foreach (IList ll in list)
            {
                ret = Gen2DimData(ret, ll);
            }

            return ret;
        }

        /// <summary>
        /// ��ά�ȴ洢��ʽת���ɶ�ά���ʽ
        /// </summary>
        /// <param name="vStyleList">��ͼ��ʽ����</param>
        /// <param name="checkedDim">ѡ�е�ά��ֵ</param>
        /// <returns></returns>
        public IList Gen2DimTable(IList vStyleList,Hashtable checkedDim)
        {
            IList dimsList = new ArrayList();//ά�ȼ���
            foreach (ViewStyle vs in vStyleList)
            {
                if (vs.Direction.Equals("��"))
                {
                    IList valueList = new ArrayList();
                    IList indicators = new ArrayList();
                    if (checkedDim[vs.Id] != null)
                    {
                        indicators = checkedDim[vs.Id] as IList;
                    }
                    foreach (ViewStyleDimension vsd in indicators)
                    {
                        valueList.Add(vsd.Name);
                    }
                    dimsList.Add(valueList);
                }
            }

            IList ret = new ArrayList();

            foreach (IList dims in dimsList)
            {
                ret = Gen2DimData(ret, dims);
            }

            return ret;
        }

        /// <summary>
        /// ����ָ��ģ��Ļ�������ֵ�γ�������
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="view"></param>
        /// <param name="selected"></param>
        public void InitialIndicatorCombox(string[] code,string[] name, ComboBox view, bool selected)
        {
            IList list = new ArrayList();
            for (int i = 0; i < code.Length; i++)
            {
                IndicatorBasicValue ibv = new IndicatorBasicValue();
                ibv.Code = code[i].ToString();
                ibv.Name = name[i].ToString();
                list.Add(ibv);
            }

            if (selected == false)
            {
                IndicatorBasicValue ibv = new IndicatorBasicValue();
                ibv.Code = "";
                ibv.Name = "--��ѡ��--";
                list.Insert(0, ibv);
            }

            view.DataSource = list;
            view.DisplayMember = "Name";
            view.ValueMember = "Code";
        }

        /// <summary>
        /// ����ָ��ģ��Ļ�������ֵ�γɼ���
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="view"></param>
        /// <param name="selected"></param>
        public IList InitialIndicatorList(string[] code, string[] name,bool selected)
        {
            IList list = new ArrayList();
            for (int i = 0; i < code.Length; i++)
            {
                IndicatorBasicValue ibv = new IndicatorBasicValue();
                ibv.Code = code[i].ToString();
                ibv.Name = name[i].ToString();
                list.Add(ibv);
            }

            if (selected == false)
            {
                IndicatorBasicValue ibv = new IndicatorBasicValue();
                ibv.Code = "";
                ibv.Name = "--��ѡ��--";
                list.Insert(0, ibv);
            }
            return list;
        }

        //ת��ά�ȵ�ID������
        public string TransIdToName(string id_link, Hashtable ht_dim)
        {
            //��һ��ȡ"[]"�е��ַ�
            string temp_str = "";
            char[] patten1 = { '[', ']' };//�ָ���
            string[] temp1 = id_link.Split(patten1);
            for (int t = 1; t < temp1.Length - 1; t = t + 2)
            {
                temp_str += temp1[t].ToString();
            }
            //�ڶ���ȡά��ֵID
            char[] patten2 = { '_' };//�ָ���
            string[] temp2 = temp_str.Split(patten2);
            for (int t = 1; t < temp2.Length; t++)
            {
                string id = temp2[t].ToString();
                if (KnowledgeUtil.IfNumber(id) == true)
                {
                    id_link = id_link.Replace(id, KnowledgeUtil.TransToString(ht_dim[id]));
                }
            }
            return id_link;
        }
    }
}
