using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyPay
{
    public class Element
    {

        //根据Name获取元素
        public HtmlElement GetElement_Name(WebBrowser wb, string Name)
        {
            HtmlElement e = wb.Document.All[Name];
            return e;
        }

        //根据Id获取元素
        public HtmlElement GetElement_Id(WebBrowser wb, string id)
        {
            HtmlElement e = wb.Document.GetElementById(id);
            return e;
        }

        //根据Index获取元素
        public HtmlElement GetElement_Index(WebBrowser wb, int index)
        {
            HtmlElement e = wb.Document.All[index];
            return e;
        }

        // 据Type获取元 ，在没有NAME和ID的情况下使用
        public HtmlElement GetElement_Type(WebBrowser wb, string type)
        {
            HtmlElement e = null;
            HtmlElementCollection elements = wb.Document.GetElementsByTagName("input");
            foreach (HtmlElement element in elements)
            {
                if (element.GetAttribute("type") == type)
                {
                    e = element;
                }
            }
            return e;
        }
        // 据Type获取元 ，在没有NAME和ID的情况下使用,并指定是同类type的第 个，GetElement_Type（）升级版
        public HtmlElement GetElement_Type_No(WebBrowser wb, string type, int i)
        {
            int j = 1;
            HtmlElement e = null;
            HtmlElementCollection elements = wb.Document.GetElementsByTagName("input");
            foreach (HtmlElement element in elements)
            {
                if (element.GetAttribute("type") == type)
                {
                    if (j == i)
                    {
                        e = element;
                    }
                    j++;
                }
            }
            return e;
        }
        //获取form表单名name,返回表单
        public HtmlElement GetElement_Form(WebBrowser wb, string form_name)
        {
            HtmlElement e = wb.Document.Forms[form_name];
            return e;
        }


        //设置元素value属性的值
        public void Write_value(HtmlElement e, string value)
        {
            e.SetAttribute("value", value);
        }

        //执行元素的方法，如：click，submit(需Form表单名)等
        public void Btn_click(HtmlElement e, string s)
        {

            e.InvokeMember(s);
        }

    }
}
