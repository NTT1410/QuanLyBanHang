﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyKhoHang.Forms
{
    public partial class frmImport : Form
    {
        public frmImport()
        {
            InitializeComponent();
        }
        List<HDNhap> hDNhaps = new List<HDNhap>();
        List<Product> products = new List<Product>();
        DataTable dtb;
        DataTable tempHDNhap;
        private void btnSearch_Click(object sender, EventArgs e)
        {
            //txtTest.Text = dateTimePicker1.Value.ToString();
            //string path = Application.StartupPath + @"\Source\DBTest.json";
            //string json = JsonConvert.SerializeObject(txtTest.Text);
            //File.WriteAllText(path, json);
            //DateTime dt = DateTime.Parse("02/10/2012 10:15:53 CH");
            //dateTimePicker1.Value = dt;
            searchHDNhap();
            if (!searchHDNhap())
            {
                MessageBox.Show("Không tìm thấy!!!");
                dgvHDNhap.DataSource = dtb;
            }

        }
        bool searchHDNhap()
        {
            tempHDNhap = new DataTable();
            addHDNhap(tempHDNhap);
            DataRow row;
            foreach (HDNhap h in hDNhaps)
            {
                if (DateTime.Parse(h.NgayNhap) >= time1.Value & DateTime.Parse(h.NgayNhap) <= time2.Value)
                {
                    int count = 0;
                    int Dsum = 0;
                    row = tempHDNhap.NewRow();
                    row[0] = h.IDNhap;
                    row[1] = h.NgayNhap;
                    foreach (HangNhap hn in h.HangNhap)
                    {
                        foreach (Product p in products)
                        {
                            if (hn.ProductID == p.ProductID)
                                Dsum = Dsum + int.Parse(hn.Quantity) * int.Parse(p.UnitPrice);
                        }
                        count++;
                    }
                    row[2] = count;
                    row[3] = Dsum.ToString("#,###");
                    tempHDNhap.Rows.Add(row);
                }
            }
            if (tempHDNhap != null)
                if(tempHDNhap.Rows.Count > 0)
                    return true;
            return false;
        }

        private void frmImport_Load(object sender, EventArgs e)
        {
            time2.Value = DateTime.Now.Date;
            dtb = new DataTable();
            try
            {
                string path = Application.StartupPath + @"\Source\DBTest.json";
                string path2 = Application.StartupPath + @"\Source\DBProducts.json";
                products = ListProduct.readFile(path2);
                hDNhaps = ListHDNhap.readFile(path);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            addHDNhap(dtb);
            DataRow row;
            foreach (HDNhap h in hDNhaps)
            {
                int count = 0;
                int Dsum = 0;
                row = dtb.NewRow();
                row[0] = h.IDNhap;
                row[1] = h.NgayNhap;
                foreach (HangNhap hn in h.HangNhap)
                {
                    foreach(Product p in products)
                    {
                        if (hn.ProductID == p.ProductID)
                            Dsum = Dsum + int.Parse(hn.Quantity) * int.Parse(p.UnitPrice);
                    }
                    count++;
                }
                row[2] = count;
                row[3] = Dsum.ToString("#,###");
                dtb.Rows.Add(row);
            }
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {

        }

        private void addHDNhap(DataTable dtb)
        {
            dtb.Columns.Add("IDNhap");
            dtb.Columns.Add("NgayNhap");
            dtb.Columns.Add("Count");
            dtb.Columns.Add("Sum");
            dgvHDNhap.DataSource = dtb;
            dgvHDNhap.Columns[0].Width = (int)(dgvHDNhap.Width * 0.25);
            dgvHDNhap.Columns[1].Width = (int)(dgvHDNhap.Width * 0.25);
            dgvHDNhap.Columns[2].Width = (int)(dgvHDNhap.Width * 0.20);
            dgvHDNhap.Columns[3].Width = (int)(dgvHDNhap.Width * 0.25);
        }

        private void btnVIewAll_Click(object sender, EventArgs e)
        {
            time1.Value = DateTime.Parse("01/01/2000 10:15 CH");
            dgvHDNhap.DataSource = dtb;
        }

        private void btnNhap_Click(object sender, EventArgs e)
        {
            frmImportChild f = new frmImportChild();
            f.ShowDialog();
        }
    }
}
