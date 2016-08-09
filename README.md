# ScrapeMe
Take what you want, when you want.


using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using EntityFramework.BulkInsert.Extensions;
using Newtonsoft.Json;
using System.Configuration;
namespace Scrapper
{
    public partial class Form1 : Form
    {
        #region MEMBER VARIABLES
        const string _strUserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:31.0) Gecko/20100101 Firefox/31.0";
        const string _strWorking = "Working...";
        const string _strStopped = "Stopped";
        public XYThreadPool _threadPool;
        public Thread _threadCheckPoolCount;
        public delegate void ThreadAddDelegate(string URL);
        public delegate void ThreadCheckDelegate(String pStrObj);
        public string ProductDescriptionURL = "http://www.publix.com/product%20catalog/ProductContent?pid={0}&tab=overview&storeid=" + ConfigurationManager.AppSettings["STOREID"];
        public string ProductAisleURL = "http://services.publix.com/api/v1/retailsubsection/GetLocation/{0}?storeNumber=" + ConfigurationManager.AppSettings["STOREID"];
        public string ProductStoreURL = "http://www.publix.com/all-products/?SetStoreId=" + ConfigurationManager.AppSettings["STOREID"];
        public string cookieee = string.Empty;
        public List<CategoryBox> CategoryList;
        #endregion

        #region FORM EVENTS
        public Form1()
        {
            InitializeComponent();
            _threadPool = new XYThreadPool();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            wbMain.Navigate(ProductStoreURL);
            dgvProduct.Visible = false;
            wbMain.Visible = true;
        }
        
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (true)
                {
                    try
                    {
                        _threadPool.StopThreadPool();
                        if (_threadCheckPoolCount != null)
                            _threadCheckPoolCount.Abort();
                        if (btnStart.Text.ToLower() == "start scrapper")
                        {
                            _threadPool.StartThreadPool(0, 1);
                            SetLabelText("Run");
                            cookieee = wbMain.Document.Cookie;
                            GetScrapingResults();
                        }
                        else if (btnStart.Text.ToLower() == "stop scrapper")
                        {
                            _threadPool.StopThreadPool();
                            SetLabelText("Stopped");
                        }
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
            }
            catch (Exception ex) { }
        }
        
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _threadPool.StopThreadPool();
            if (_threadCheckPoolCount != null)
                _threadCheckPoolCount.Abort();
            Application.ExitThread();
            Application.Exit(new CancelEventArgs(true));
        }
        
        private void aboutUsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("PUBLIX DATA", "About Us", MessageBoxButtons.OK);
        }
        
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _threadPool.StopThreadPool();
            if (_threadCheckPoolCount != null)
                _threadCheckPoolCount.Abort();
            Application.ExitThread();
            Application.Exit(new CancelEventArgs(true));
        }
        
        private void btnGetCategories_Click(object sender, EventArgs e)
        {
            dgvProduct.Visible = false;
            wbMain.Visible = true;
            wbMain.Navigate(ProductStoreURL);
        }
        #endregion

        #region THREAD METHODS
        void DoWorkForPoolCount()
        {
            while (true)
            {
                if (_threadPool.GetThreadCount() == 0)
                {
                    SetLabelText("Stopped");
                    _threadCheckPoolCount.Abort();
                }
                else
                {
                    SetLabelText("Run");
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                }
            }
        }

        void GetFastProduct(Object pObject)
        {
            string strData = (string)pObject;
            CategoryList = new List<CategoryBox>();
            int Counter = 1;
            if (cbxGetCategories.Checked)
            {
                GetCategory();
                GetSubCategory();
                GetSubSubCategory();
                GetSubSubSubCategory();
            }
            if (cbxGetProducts.Checked)
            {
                using (ScrapperEntities _context = new ScrapperEntities())
                {
                    var cList = _context.Publix_Category.Where(c => c.IsParent == false && c.IsScrapped == false).ToList();
                    var csList = _context.Publix_SubCategory.Where(c => c.IsParent == false && c.IsScrapped == false).ToList();
                    var cssList = _context.Publix_SubSubCategory.Where(c => c.IsParent == false && c.IsScrapped == false).ToList();
                    var csssList = _context.Publix_SubSubSubCategory.Where(c => c.IsParent == false && c.IsScrapped == false).ToList();
                    foreach (var cl in cList)
                    {
                        CategoryList.Add(new CategoryBox(Counter, cl.ID, cl.Name, cl.URL.Replace("page=1&", "page={0}&count=96"), "Category"));
                        Counter++;
                    }
                    foreach (var cl in csList)
                    {
                        CategoryList.Add(new CategoryBox(Counter, cl.ID, cl.Name, cl.URL.Replace("page=1&", "page={0}&count=96"), "SubCategory"));
                        Counter++;
                    }
                    foreach (var cl in cssList)
                    {
                        CategoryList.Add(new CategoryBox(Counter, cl.ID, cl.Name, cl.URL.Replace("page=1&", "page={0}&count=96"), "SubSubCategory"));
                        Counter++;
                    }
                    foreach (var cl in csssList)
                    {
                        CategoryList.Add(new CategoryBox(Counter, cl.ID, cl.Name, cl.URL.Replace("page=1&", "page={0}&count=96"), "SubSubSubCategory"));
                        Counter++;
                    }
                }
                foreach (var cc in CategoryList)
                {
                    int pageno = 1;
                    bool Flag = true;
                    while (Flag)
                    {
                        GetProductList(string.Format(cc.URL, pageno), cc.ID, cc.CategoryType, out Flag);
                        pageno++;
                    }
                    if (cc.CategoryType == "Category")
                    {
                        using (ScrapperEntities _context = new ScrapperEntities())
                        {
                            var pbp = _context.Publix_Category.Where(pp => pp.ID == cc.ID).First();
                            pbp.IsScrapped = true;
                            _context.SaveChanges();
                        }
                    }
                    else if (cc.CategoryType == "SubCategory")
                    {
                        using (ScrapperEntities _context = new ScrapperEntities())
                        {
                            var pbp = _context.Publix_SubCategory.Where(pp => pp.ID == cc.ID).First();
                            pbp.IsScrapped = true;
                            _context.SaveChanges();
                        }
                    }
                    else if (cc.CategoryType == "SubSubCategory")
                    {
                        using (ScrapperEntities _context = new ScrapperEntities())
                        {
                            var pbp = _context.Publix_SubSubCategory.Where(pp => pp.ID == cc.ID).First();
                            pbp.IsScrapped = true;
                            _context.SaveChanges();
                        }
                    }
                    else if (cc.CategoryType == "SubSubSubCategory")
                    {
                        using (ScrapperEntities _context = new ScrapperEntities())
                        {
                            var pbp = _context.Publix_SubSubSubCategory.Where(pp => pp.ID == cc.ID).First();
                            pbp.IsScrapped = true;
                            _context.SaveChanges();
                        }
                    }
                }
            }
            List<Publix_Product> proList = new List<Publix_Product>();
            using (ScrapperEntities _context = new ScrapperEntities())
            {
                proList = _context.Publix_Product.Where(c => c.Description == null && c.AisleName == null).ToList();
            }
            foreach (var pl in proList)
            {
                GetItemCodeAisleID(pl.ProductURL, pl.ID, pl.UPC);
            }
        }
        #endregion

        #region FORM METHODS
        void GetScrapingResults()
        {
            try
            {
                _threadPool.InsertWorkItem("Parent", new ThreadAddDelegate(GetFastProduct), new object[1] { "" }, true);
                _threadCheckPoolCount = new Thread(new ThreadStart(DoWorkForPoolCount));
                _threadCheckPoolCount.Start();
            }
            catch { }
        }
        
        void GetCategory()
        {
            GET_WEB_CATEGORY(ProductStoreURL);
        }
        
        void GetSubCategory()
        {
            ScrapperEntities _context = new ScrapperEntities();
            var cList = _context.Publix_Category.Where(c => c.IsParent == true).ToList();
            foreach (var cl in cList)
            {
                GET_WEB_SUB_CATEGORY(cl.URL, cl.ID);
            }
        }
        
        void GetSubSubCategory()
        {
            ScrapperEntities _context = new ScrapperEntities();
            var cList = _context.Publix_SubCategory.Where(c => c.IsParent == true).ToList();
            foreach (var cl in cList)
            {
                GET_WEB_SUB_SUB_CATEGORY(cl.URL, cl.ID);
            }
        }
        
        void GetSubSubSubCategory()
        {
            ScrapperEntities _context = new ScrapperEntities();
            var cList = _context.Publix_SubSubCategory.Where(c => c.IsParent == true).ToList();
            foreach (var cl in cList)
            {
                GET_WEB_SUB_SUB_SUB_CATEGORY(cl.URL, cl.ID);
            }
        }
        
        public string StripTagsRegex(string source)
        {
            source = Regex.Replace(source, "<.*?>", string.Empty).Trim();
            return HtmlAgilityPack.HtmlEntity.DeEntitize(source).Trim();
        }
        
        void GetJSON(string URL, int proID, string proType)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                using (WebResponse resp = request.GetResponse())
                {
                    StreamReader reader = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);
                    String data = reader.ReadToEnd();
                    if (data.Length > 10)
                    {
                        var aisleDetail = JsonConvert.DeserializeObject<AisleInfo>(data);
                        if (aisleDetail != null)
                        {
                            using (ScrapperEntities _context = new ScrapperEntities())
                            {
                                var pbp = _context.Publix_Product.Where(pp => pp.ID == proID).First();
                                pbp.AisleName = aisleDetail.Location;
                                _context.SaveChanges();
                            }
                        }
                    }
                }
            }
            catch { }
            GetDescription(string.Format(ProductDescriptionURL, proType), proID);
        }
        
        void GetDescription(string URL, int proID)
        {
            try
            {
                HtmlAgilityPack.HtmlDocument hDoc = new HtmlAgilityPack.HtmlDocument();
                WebClient wClient = new WebClient();
                wClient.Headers.Add("Cookie", cookieee);
                wClient.Headers.Add("user-agent", _strUserAgent);
                byte[] bData = wClient.DownloadData(URL);
                hDoc.LoadHtml(ASCIIEncoding.UTF8.GetString(bData));
                string Description = string.Empty;
                try
                {
                    Description = StripTagsRegex(hDoc.DocumentNode.SelectSingleNode("//span[@id='content_0_OverviewRepeater_OverviewText_0']").InnerHtml).Trim();
                }
                catch (Exception ex)
                {
                }
                using (ScrapperEntities _context = new ScrapperEntities())
                {
                    var pbp = _context.Publix_Product.Where(pp => pp.ID == proID).First();
                    pbp.Description = Description;
                    _context.SaveChanges();
                }
            }
            catch { }
        }
        
        void GetItemCodeAisleID(string URL, int proID, string proType)
        {
            try
            {
                HtmlAgilityPack.HtmlDocument hDoc = new HtmlAgilityPack.HtmlDocument();
                WebClient wClient = new WebClient();
                wClient.Headers.Add("Cookie", cookieee);
                wClient.Headers.Add("user-agent", _strUserAgent);
                byte[] bData = wClient.DownloadData(URL);
                hDoc.LoadHtml(ASCIIEncoding.UTF8.GetString(bData));
                string ItemCode = string.Empty, AisleID = string.Empty;
                try
                {
                    ItemCode = StripTagsRegex(hDoc.DocumentNode.SelectSingleNode("//input[@id='content_1_3fourthswidth2colright_0_ProductQuantity_ItemCode']").Attributes["value"].Value).Trim();
                }
                catch (Exception ex)
                {
                }
                try
                {
                    AisleID = StripTagsRegex(hDoc.DocumentNode.SelectSingleNode("//input[@id='content_1_3fourthswidth2colright_0_ProductQuantity_aisleId']").Attributes["value"].Value).Trim();
                }
                catch (Exception ex)
                {
                }
                using (ScrapperEntities _context = new ScrapperEntities())
                {
                    var pbp = _context.Publix_Product.Where(pp => pp.ID == proID).First();
                    pbp.ItemCode = ItemCode;
                    pbp.AisleID = AisleID;
                    _context.SaveChanges();
                }
                GetJSON(string.Format(ProductAisleURL, AisleID), proID, proType);
            }
            catch { }
        }
        
        void GetProductList(string PubURL, int PubCategoryID, string PubCategoryType, out bool Flag)
        {
            Thread.Sleep(500);
            Flag = false;
            HtmlAgilityPack.HtmlDocument hDoc = new HtmlAgilityPack.HtmlDocument();
            WebClient wClient = new WebClient();
            wClient.Headers.Add("Cookie", cookieee);
            wClient.Headers.Add("user-agent", _strUserAgent);
            byte[] bData = wClient.DownloadData(PubURL);
            hDoc.LoadHtml(ASCIIEncoding.UTF8.GetString(bData));
            string Name = string.Empty, UPC = string.Empty, ImageURL = string.Empty, Size = string.Empty, URL = string.Empty, Price = string.Empty;
            var listNodes = hDoc.DocumentNode.SelectNodes("//div[@class='product tablefullwidth']");
            if (listNodes != null)
            {
                List<Publix_Product> lstPublixCategory = new List<Publix_Product>();
                foreach (HtmlAgilityPack.HtmlNode hn in listNodes)
                {
                    Flag = true;
                    try
                    {
                        Name = string.Empty; UPC = string.Empty; ImageURL = string.Empty; Size = string.Empty; URL = string.Empty; Price = string.Empty;
                        HtmlAgilityPack.HtmlDocument childDoc = new HtmlAgilityPack.HtmlDocument();
                        childDoc.LoadHtml(hn.InnerHtml);
                        try
                        {
                            Name = StripTagsRegex(childDoc.DocumentNode.SelectSingleNode("//div[@class='item-description']/div/h2/a/span").InnerHtml).Trim();
                        }
                        catch { }
                        try
                        {
                            URL = StripTagsRegex(childDoc.DocumentNode.SelectSingleNode("//div[@class='item-description']/div/h2/a").Attributes["href"].Value).Trim();
                        }
                        catch { }
                        try
                        {
                            UPC = StripTagsRegex(childDoc.DocumentNode.SelectSingleNode("//button[@class='add-to-list btn']").Attributes["productid"].Value).Trim();
                        }
                        catch { }
                        try
                        {
                            ImageURL = StripTagsRegex(childDoc.DocumentNode.SelectSingleNode("//div[@class='product-info']/div/div[@class='main-image']/a/img").Attributes["src"].Value).Trim();
                        }
                        catch { }
                        try
                        {
                            Size = StripTagsRegex(childDoc.DocumentNode.SelectSingleNode("//div[@class='item-description']/div[@class='sizedescription']").InnerHtml).Trim();
                        }
                        catch { }
                        try
                        {
                            Price = StripTagsRegex(childDoc.DocumentNode.SelectSingleNode("//div[@class='item-description']/div[@class='priceline-1-wrapper']/span[@class='priceline1']").InnerHtml).Trim();
                        }
                        catch { }
                        try
                        {
                            URL = URL.Contains("www.publix.com") ? URL : "http://www.publix.com" + URL;
                        }
                        catch { }
                        try
                        {
                            string[] arrrURL = URL.Split('/');
                            if (arrrURL.Length > 4)
                            {
                                UPC = arrrURL[4].Trim();
                                if (UPC.Contains('?'))
                                {
                                    arrrURL = UPC.Split('?');
                                    UPC = arrrURL[0];
                                }
                            }
                        }
                        catch { }
                        if (!string.IsNullOrEmpty(Name))
                        {
                            lstPublixCategory.Add(new Publix_Product
                            {
                                Name = Name,
                                CategoryType = PubCategoryType,
                                Price = Price,
                                ProductURL = URL,
                                CatID = PubCategoryID,
                                Image = ImageURL,
                                Size = Size,
                                UPC = UPC
                            });
                        }
                    }
                    catch { }
                }
                using (ScrapperEntities _context = new ScrapperEntities())
                {
                    _context.BulkInsert(lstPublixCategory);
                    _context.SaveChanges();
                }
            }
        }
        
        private void GET_WEB_CATEGORY(string CATURL)
        {
            try
            {
                HtmlAgilityPack.HtmlDocument hDoc = new HtmlAgilityPack.HtmlDocument();
                WebClient wClient = new WebClient();
                wClient.Headers.Add("Cookie", cookieee);
                wClient.Headers.Add("user-agent", _strUserAgent);
                byte[] bData = wClient.DownloadData(CATURL);
                hDoc.LoadHtml(ASCIIEncoding.UTF8.GetString(bData));
                string Name = string.Empty, UPC = string.Empty, ImageURL = string.Empty, Size = string.Empty, URL = string.Empty, Price = string.Empty;
                var listNodes = hDoc.DocumentNode.SelectNodes("//div[@id='catalog-left-nav']/ul/li/a").Where(a => a.InnerHtml.Trim() != "Weekly Ad" && a.InnerHtml.Trim() != "BOGO Specials" && a.InnerHtml.Trim() != "Online Easy Ordering").ToList();
                if (listNodes != null)
                {
                    List<Publix_Category> lstPublixCategory = new List<Publix_Category>();
                    foreach (HtmlAgilityPack.HtmlNode hn in listNodes)
                    {
                        try
                        {
                            Name = string.Empty; URL = string.Empty; Price = string.Empty;
                            HtmlAgilityPack.HtmlDocument childDoc = new HtmlAgilityPack.HtmlDocument();
                            childDoc.LoadHtml(hn.OuterHtml);
                            try
                            {
                                Name = StripTagsRegex(childDoc.DocumentNode.SelectSingleNode("//a").InnerHtml).Trim();
                            }
                            catch { }
                            try
                            {
                                URL = StripTagsRegex(childDoc.DocumentNode.SelectSingleNode("//a").Attributes["href"].Value).Trim();
                            }
                            catch { }
                            if (!string.IsNullOrEmpty(Name))
                            {
                                lstPublixCategory.Add(new Publix_Category
                                {
                                    Name = Name,
                                    URL = URL.Contains("www.publix.com") ? URL : "http://www.publix.com" + URL,
                                    IsParent = URL.Contains("/product-catalog/productlisting") ? false : true
                                });
                            }
                        }
                        catch { }
                    }
                    using (ScrapperEntities _context = new ScrapperEntities())
                    {
                        _context.BulkInsert(lstPublixCategory);
                        _context.SaveChanges();
                    }
                }
            }
            catch { }
        }
        
        private void GET_WEB_SUB_CATEGORY(string SUBCATURL, int CATID)
        {
            try
            {
                HtmlAgilityPack.HtmlDocument hDoc = new HtmlAgilityPack.HtmlDocument();
                WebClient wClient = new WebClient();
                wClient.Headers.Add("Cookie", cookieee);
                wClient.Headers.Add("user-agent", _strUserAgent);
                byte[] bData = wClient.DownloadData(SUBCATURL);
                hDoc.LoadHtml(ASCIIEncoding.UTF8.GetString(bData));
                string Name = string.Empty, UPC = string.Empty, ImageURL = string.Empty, Size = string.Empty, URL = string.Empty, Price = string.Empty;
                var listNodes = hDoc.DocumentNode.SelectNodes("//div[@id='catalog-left-nav']/ul/li/a").Where(a => a.InnerHtml.Trim() != "Weekly Ad" && a.InnerHtml.Trim() != "BOGO Specials" && a.InnerHtml.Trim() != "Online Easy Ordering").ToList();
                if (listNodes != null)
                {
                    List<Publix_SubCategory> lstPublixCategory = new List<Publix_SubCategory>();
                    foreach (HtmlAgilityPack.HtmlNode hn in listNodes)
                    {
                        try
                        {
                            Name = string.Empty; URL = string.Empty; Price = string.Empty;
                            HtmlAgilityPack.HtmlDocument childDoc = new HtmlAgilityPack.HtmlDocument();
                            childDoc.LoadHtml(hn.OuterHtml);
                            try
                            {
                                Name = StripTagsRegex(childDoc.DocumentNode.SelectSingleNode("//a").InnerHtml).Trim();
                            }
                            catch { }
                            try
                            {
                                URL = StripTagsRegex(childDoc.DocumentNode.SelectSingleNode("//a").Attributes["href"].Value).Trim();
                            }
                            catch { }
                            if (!string.IsNullOrEmpty(Name))
                            {
                                lstPublixCategory.Add(new Publix_SubCategory
                                {
                                    Name = Name,
                                    URL = URL.Contains("www.publix.com") ? URL : "http://www.publix.com" + URL,
                                    IsParent = URL.Contains("/product-catalog/productlisting") ? false : true,
                                    CategoryID = CATID
                                });
                            }
                        }
                        catch { }
                    }
                    using (ScrapperEntities _context = new ScrapperEntities())
                    {
                        _context.BulkInsert(lstPublixCategory);
                        _context.SaveChanges();
                    }
                }
            }
            catch { }
        }
        
        private void GET_WEB_SUB_SUB_CATEGORY(string SUBSUBCATURL, int SUBCATID)
        {
            try
            {
                HtmlAgilityPack.HtmlDocument hDoc = new HtmlAgilityPack.HtmlDocument();
                WebClient wClient = new WebClient();
                wClient.Headers.Add("Cookie", cookieee);
                wClient.Headers.Add("user-agent", _strUserAgent);
                byte[] bData = wClient.DownloadData(SUBSUBCATURL);
                hDoc.LoadHtml(ASCIIEncoding.UTF8.GetString(bData));
                string Name = string.Empty, UPC = string.Empty, ImageURL = string.Empty, Size = string.Empty, URL = string.Empty, Price = string.Empty;
                var listNodes = hDoc.DocumentNode.SelectNodes("//div[@id='catalog-left-nav']/ul/li/a").Where(a => a.InnerHtml.Trim() != "Weekly Ad" && a.InnerHtml.Trim() != "BOGO Specials" && a.InnerHtml.Trim() != "Online Easy Ordering").ToList();
                if (listNodes != null)
                {
                    List<Publix_SubSubCategory> lstPublixCategory = new List<Publix_SubSubCategory>();
                    foreach (HtmlAgilityPack.HtmlNode hn in listNodes)
                    {
                        try
                        {
                            Name = string.Empty; URL = string.Empty; Price = string.Empty;
                            HtmlAgilityPack.HtmlDocument childDoc = new HtmlAgilityPack.HtmlDocument();
                            childDoc.LoadHtml(hn.OuterHtml);
                            try
                            {
                                Name = StripTagsRegex(childDoc.DocumentNode.SelectSingleNode("//a").InnerHtml).Trim();
                            }
                            catch { }
                            try
                            {
                                URL = StripTagsRegex(childDoc.DocumentNode.SelectSingleNode("//a").Attributes["href"].Value).Trim();
                            }
                            catch { }
                            if (!string.IsNullOrEmpty(Name))
                            {
                                lstPublixCategory.Add(new Publix_SubSubCategory
                                {
                                    Name = Name,
                                    URL = URL.Contains("www.publix.com") ? URL : "http://www.publix.com" + URL,
                                    IsParent = URL.Contains("/product-catalog/productlisting") ? false : true,
                                    SubCategoryID = SUBCATID
                                });
                            }
                        }
                        catch { }
                    }
                    using (ScrapperEntities _context = new ScrapperEntities())
                    {
                        _context.BulkInsert(lstPublixCategory);
                        _context.SaveChanges();
                    }
                }
            }
            catch { }
        }
        
        private void GET_WEB_SUB_SUB_SUB_CATEGORY(string SUBSUBSUBCATURL, int SUBSUBCATID)
        {
            try
            {
                HtmlAgilityPack.HtmlDocument hDoc = new HtmlAgilityPack.HtmlDocument();
                WebClient wClient = new WebClient();
                wClient.Headers.Add("Cookie", cookieee);
                wClient.Headers.Add("user-agent", _strUserAgent);
                byte[] bData = wClient.DownloadData(SUBSUBSUBCATURL);
                hDoc.LoadHtml(ASCIIEncoding.UTF8.GetString(bData));
                string Name = string.Empty, UPC = string.Empty, ImageURL = string.Empty, Size = string.Empty, URL = string.Empty, Price = string.Empty;
                var listNodes = hDoc.DocumentNode.SelectNodes("//div[@id='catalog-left-nav']/ul/li/a").Where(a => a.InnerHtml.Trim() != "Weekly Ad" && a.InnerHtml.Trim() != "BOGO Specials" && a.InnerHtml.Trim() != "Online Easy Ordering").ToList();
                if (listNodes != null)
                {
                    List<Publix_SubSubSubCategory> lstPublixCategory = new List<Publix_SubSubSubCategory>();
                    foreach (HtmlAgilityPack.HtmlNode hn in listNodes)
                    {
                        try
                        {
                            Name = string.Empty; URL = string.Empty; Price = string.Empty;
                            HtmlAgilityPack.HtmlDocument childDoc = new HtmlAgilityPack.HtmlDocument();
                            childDoc.LoadHtml(hn.OuterHtml);
                            try
                            {
                                Name = StripTagsRegex(childDoc.DocumentNode.SelectSingleNode("//a").InnerHtml).Trim();
                            }
                            catch { }
                            try
                            {
                                URL = StripTagsRegex(childDoc.DocumentNode.SelectSingleNode("//a").Attributes["href"].Value).Trim();
                            }
                            catch { }
                            if (!string.IsNullOrEmpty(Name))
                            {
                                lstPublixCategory.Add(new Publix_SubSubSubCategory
                                {
                                    Name = Name,
                                    URL = URL.Contains("www.publix.com") ? URL : "http://www.publix.com" + URL,
                                    IsParent = URL.Contains("/product-catalog/productlisting") ? false : true,
                                    SubSubCategoryID = SUBSUBCATID
                                });
                            }
                        }
                        catch { }
                    }
                    using (ScrapperEntities _context = new ScrapperEntities())
                    {
                        _context.BulkInsert(lstPublixCategory);
                        _context.SaveChanges();
                    }
                }
            }
            catch { }
        }
        #endregion

        #region FORM DELEGATES
        private delegate string SetLabelTextDelegate(string pStrMessage);
        private string SetLabelText(string pStrMessage)
        {
            try
            {
                if (InvokeRequired)
                {
                    SetLabelTextDelegate objDelg = new SetLabelTextDelegate(SetLabelText);
                    return Invoke(objDelg, pStrMessage).ToString();
                }
                else
                {
                    if (pStrMessage == "Run")
                    {
                        btnStart.Text = "Stop Scrapper";
                    }
                    else
                    {
                        btnStart.Text = "Start Scrapper";
                    }
                    return "success";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        #endregion
    }
    #region SUPPORT CLASS
    public class Box
    {
        public Box(string pName, string pValue)
        {
            this.Name = pName;
            this.Value = pValue;
        }
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class AisleInfo
    {
        public string Location { get; set; }
        public string MobileLocation1 { get; set; }
        public string MobileLocation2 { get; set; }
    }
    public class CategoryBox
    {
        public CategoryBox(int pCID, int pID, string pCategory, string pURL, string pCategoryType)
        {
            this.CID = pCID;
            this.ID = pID;
            this.Category = pCategory;
            this.URL = pURL;
            this.CategoryType = pCategoryType;
        }
        public int CID { get; set; }
        public int ID { get; set; }
        public string URL { get; set; }
        public string Category { get; set; }
        public string CategoryType { get; set; }
    }
    #endregion
