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
        /// 视图服务
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
        /// 立方(主题)服务
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
        /// 维度定义服务
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
        /// 把维度数组转化为二维表格式
        /// </summary>
        /// <param name="startDimList">存储二维表结构维度的集合</param>
        /// <param name="endDimList">单一维度的集合</param>
        /// <returns></returns>
        private IList Gen2DimData(IList startDimList, IList endDimList)
        {
            IList ret = new ArrayList();

            //如果startDimList为空，则把endDimList直接加到startDimList
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
                //如果startDimList中已经有结构数据
                foreach (object dimObj in startDimList)
                {
                    foreach (string dim in endDimList)
                    {
                        if (dimObj is IList)
                        {
                            //如果是集合则把集合中的值与新的维度值保存在一个新的集合中
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
        /// 把维度存储格式转换成二维表格式
        /// </summary>
        /// <param name="vStyleList">视图格式数组</param>
        /// <param name="displayAttr">显示属性 值为Id则显示原分类树Id,否则显示名称</param>
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
        /// 把维度存储格式转换成二维表格式
        /// </summary>
        /// <param name="vStyleList">视图格式数组</param>
        /// <param name="checkedDim">选中的维度值</param>
        /// <returns></returns>
        public IList Gen2DimTable(IList vStyleList,Hashtable checkedDim)
        {
            IList dimsList = new ArrayList();//维度集合
            foreach (ViewStyle vs in vStyleList)
            {
                if (vs.Direction.Equals("行"))
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
        /// 根据指标模块的基础数据值形成下拉框
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
                ibv.Name = "--请选择--";
                list.Insert(0, ibv);
            }

            view.DataSource = list;
            view.DisplayMember = "Name";
            view.ValueMember = "Code";
        }

        /// <summary>
        /// 根据指标模块的基础数据值形成集合
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
                ibv.Name = "--请选择--";
                list.Insert(0, ibv);
            }
            return list;
        }

        //转换维度的ID到名称
        public string TransIdToName(string id_link, Hashtable ht_dim)
        {
            //第一次取"[]"中的字符
            string temp_str = "";
            char[] patten1 = { '[', ']' };//分隔符
            string[] temp1 = id_link.Split(patten1);
            for (int t = 1; t < temp1.Length - 1; t = t + 2)
            {
                temp_str += temp1[t].ToString();
            }
            //第二次取维度值ID
            char[] patten2 = { '_' };//分隔符
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
