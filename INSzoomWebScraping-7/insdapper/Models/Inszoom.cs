using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Dapper;
using HtmlAgilityPack;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras;
using insdapper.Models;


namespace insdapper.Models
{
    public class Inszoom
    {
        public static ChromeOptions opt = new ChromeOptions();
        
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        static void checkString(string xp, IWebDriver driver)
        {
            string html = driver.PageSource;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
           

            var htmlNodes = doc.DocumentNode.SelectNodes(xp);

            foreach (var node in htmlNodes)
            {
                Console.WriteLine(node.InnerText);
            }

            
        }

        static bool checkExistence(string xp, IWebDriver driver)
        {
            return driver.FindElement(By.XPath(xp)).Displayed;
        }

        public static void Webscrape()
        {
            if (CheckForInternetConnection() != true)
            {
                Console.WriteLine("NO INTERNET!");
                Console.ReadLine();
                return;
            }
            ///////////////////////////////////////////////////////////////////////
            FormUrlData f = new FormUrlData();

            List<FormUrlData> urldata = new List<FormUrlData>();
            urldata = DapperORM.url<FormUrlData>();
            String securityQuestion;
            string html;
            HtmlDocument doc = new HtmlDocument();
            opt.AddArguments("--incognito");


            foreach (FormUrlData url in urldata)
            {
               //Path for Chrome driver has to be given here
                IWebDriver driver = new ChromeDriver(@"C:\Users\Unnati\Documents\INS\INSzoomWebScraping-7\insdapper\bin\Debug\netcoreapp2.1", opt);
                opt.AddArguments("--incognito");
                driver.Navigate().GoToUrl(url.FormUrl);
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                List<CrawlerInstruction> instList = new List<CrawlerInstruction>();
                System.Console.WriteLine("fetched url" + url.FormUrl + "--------------------" + url.id);
                instList = DapperORM.instruction<CrawlerInstruction>(url.id);
                foreach (CrawlerInstruction inst in instList)
                {
                    System.Console.WriteLine("type " + inst.InstructionType + "\tValue " + inst.InstructionValue + "\tid " + inst.id);
                    
                    if (inst.InstructionType == "wait_Xpath")
                    {
                        wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(inst.InstructionValue)));
                    }
                    else if (inst.InstructionType == "wait")
                    {
                        wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id(inst.InstructionValue)));
                    }
                    else if (inst.InstructionType == "Click_Xpath")
                    {
                        driver.FindElement(By.XPath(inst.InstructionValue)).Click();
                    }
                    else if (inst.InstructionType == "Node")
                    {
                        if (inst.InstructionValue != "END")
                        {
                            html = driver.PageSource;
                            doc.LoadHtml(html);
                            securityQuestion = doc.DocumentNode.SelectSingleNode(inst.InstructionValue).InnerHtml.ToString();
                            
                        }

                    }
                    else if (inst.InstructionType == "Contains")
                    {
                        html = driver.PageSource;
                        doc.LoadHtml(html);
                        securityQuestion = doc.DocumentNode.SelectSingleNode(inst.InstructionValue).InnerHtml.ToString();
                        if (securityQuestion.Contains("favourite food"))
                            driver.FindElement(By.Id("answer")).SendKeys("Chinese");
                        else if (securityQuestion.Contains("best friend"))
                            driver.FindElement(By.Id("answer")).SendKeys("Pooja Gadia");
                        else if (securityQuestion.Contains("mobile name"))
                            driver.FindElement(By.Id("answer")).SendKeys("Pixel");
                        else if (securityQuestion.Contains("laptop name"))
                            driver.FindElement(By.Id("answer")).SendKeys("Hewlett Packard");

                    }

                    else if (inst.InstructionType == "Click_id")
                    {
                        driver.FindElement(By.Id(inst.InstructionValue)).Click();
                    }
                    else if(inst.InstructionType == "Scrape")
                    {
                        int formid = url.id;
                        NavigatePages(formid,driver);
                    }
                    else
                    {
                        driver.FindElement(By.Id(inst.InstructionType)).SendKeys(inst.InstructionValue);
                    }
                }

                //Call Navigate Pages function here
                driver.Close();
            }

            /////////////////////////////////////////////////////////////////////////


           

        }



        public static void NavigatePages(int formid,IWebDriver driver)
        {
              //next everything would come inside a loop
            int TempCount = 0; // taken to check for few pages
            while (TempCount < 15)
            {

                var CurrentPage = driver.PageSource;

                var CurrentPageDoc = new HtmlDocument();
                CurrentPageDoc.LoadHtml(CurrentPage);

                
                string name = CurrentPageDoc.DocumentNode.SelectSingleNode("//div/input").Attributes["id"].Value;
                Console.WriteLine("Availabe Button: " + name);

                if (name.Equals("_exit"))
                {
                    ExtractCurrentPage(CurrentPage,driver);
                    Console.WriteLine("I got a Next\n");
               
                    Console.WriteLine(TempCount);
                    try
                    {
                        driver.FindElement(By.Id("_next")).Click();
                        Console.WriteLine("Extract1");
                        Extract(CurrentPage,formid);
                        Console.WriteLine("Extract2");

                    }
                    catch
                    {
                        Console.WriteLine("No next time to exit the application");
                        Exit(CurrentPage,driver);
                        TempCount = 20;
                    }


                    //end of questions now  exit
                }

                TempCount++;
            }
        }

        public static void Exit(String CurrentPage, IWebDriver driver)
        {
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 10));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("_exit")));
            driver.FindElement(By.Id("_exit")).Click();
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("_continue")));
            driver.FindElement(By.Id("_continue")).Click();
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("_delete_0")));
            driver.FindElement(By.Id("_delete_0")).Click();
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("_continue")));
            driver.FindElement(By.Id("_continue")).Click();
          

        }
        public static void ExtractCurrentPage(String CurrentPage, IWebDriver driver)
        {
            var pagehtml = CurrentPage;
            Label labeldata = new Label();
            Option optiondata = new Option();
            CheckAdd change = new CheckAdd();
            HtmlDocument pagedoc = new HtmlDocument();
            pagedoc.LoadHtml(pagehtml);

            int questions_count = 0;
            int options_count = 0;
            int i = 0;
            Random rand = new Random();
            int generate = 0;
            while (i < 20)
            {
                var questions = pagedoc.DocumentNode.SelectSingleNode("//label[contains(@for,'answerlist[" + i + "]' )]");
                if (questions != null)
                {
                    questions_count++;
                    options_count = 0;
                    var answers = pagedoc.DocumentNode.SelectSingleNode("//select[contains(@id,'answerlist[" + i + "]' ) ]");
                    if (answers != null)
                    {
                        HtmlNodeCollection children = answers.ChildNodes;
                        options_count = children.Count();
                        Console.WriteLine(options_count);
                        options_count = options_count - 3;
                        Console.WriteLine("NEW COUNT: " + options_count);
                        if (options_count > 100)
                        {
                            generate = rand.Next(18, options_count);
                        }
                        else
                        {
                            generate = rand.Next(2, options_count);
                        }
                        driver.FindElement(By.XPath("//*[@id='answerlist[" + i + "]']/option[" + generate.ToString() + "]")).Click();
                        }
                }
                i++;
            }



        }
        public static void Extract(String html,int formid)
        {
            var pagehtml = html;
            // Object creation for database transfer
            
            CheckAdd change = new CheckAdd();
            HtmlDocument pagedoc = new HtmlDocument();
            pagedoc.LoadHtml(pagehtml);
            String query = "//*[@id='currentQuestionId']";

            String pageid = pagedoc.DocumentNode.SelectSingleNode(query).Attributes["value"].Value;
            int i = 0;

            HtmlNode body, answers;
            List<Label> lab = new List<Label>();
            Console.WriteLine("Inside Extract");
            while (i < 15)
            {
                body = pagedoc.DocumentNode.SelectSingleNode("//label[contains(@for,'answerlist[" + i + "]' )]");
                answers = pagedoc.DocumentNode.SelectSingleNode("//select[contains(@id,'answerlist[" + i + "]' ) ]");
                if (body == null)
                {

                }
                else
                {
                    Label labeldata = new Label();
                    String InnerLabel = body.InnerText;
                    String labelanswerid = "answerlist[" + i + "]";
                    labeldata.Label_answer_id = labelanswerid;
                    labeldata.Label_text = InnerLabel;
                    labeldata.Page_id = pageid;
                    labeldata.Formid = formid;
                    lab.Add(labeldata);
                    Console.WriteLine(formid);
                    //Updation
                    Console.WriteLine("Before Retflag");
                    int retflag = DapperORM.UpdateLabelText(labeldata);
                    Console.WriteLine("After Retflag");
                    if (retflag == 0)
                    {
                        System.Diagnostics.Debug.WriteLine("Label text same");
                       
                    }
                    else if (retflag == 1)
                    {
                        //Update the changes in Label and change table
                        System.Diagnostics.Debug.WriteLine("Label text different");
                        System.Console.WriteLine("Label_text:" + InnerLabel);
                   
                        if (DapperORM.AddUpdatedLabel(labeldata) > 0)
                        {
                           
                            change.Change_time = DateTime.Now.ToString();
                            change.Page_id = pageid;
                            change.New_id = labelanswerid;
                            change.Old_id = labelanswerid;
                            change.Formid = formid;
                            change.Change_type = "UPDATE";
                            change.Option_value = "NULL";
                            DapperORM.ChangeAdd(change);

                        }

                    }
                    else if (retflag == 2)
                    {

                        //calling the procedure
                      
                        Label checklabel = DapperORM.CheckAdd(labeldata);
                       
                       
                        if (checklabel != null)
                        {
                            System.Diagnostics.Debug.WriteLine("Tuple already exists");
                            System.Diagnostics.Debug.WriteLine("answerlist[" + i + "]");
                        }
                        else
                        {
                            if (DapperORM.AddLabel(labeldata) > 0)
                            {
                                change.Change_time = DateTime.Now.ToString();
                                change.Page_id = pageid;
                                change.New_id = labelanswerid;
                                change.Old_id = "NULL";
                                change.Formid = formid;
                                change.Change_type = "ADD";
                                change.Option_value = "NULL";
                                DapperORM.ChangeAdd(change);

                                System.Diagnostics.Debug.WriteLine("Row inserted");
                                //Adding Options
                                InsertOptions(answers, pageid, labelanswerid,formid);

                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("Error");

                            }

                        }
                        

                    }
                }
                i++;
            }

            List<Label> dbList = new List<Label>();
            dbList = DapperORM.List_With_Pageid<Label>(pageid);
           

            foreach (Label combo in dbList)
            {
                int flag1 = 0;
                combo.Label_answer_id = combo.Label_answer_id.Trim();
                foreach (Label inlab in lab)
                {
                    inlab.Label_answer_id = inlab.Label_answer_id.Trim();
                    if (string.Compare(inlab.Label_answer_id, combo.Label_answer_id) == 0)
                    {
                        flag1 = 1;
                    }

                }
                if (flag1 == 0)
                {
                    System.Console.WriteLine("answerid:" + combo.Label_answer_id + "Pageid" + combo.Page_id);
                    DapperORM.Delete(combo);
                }


            }

        }

        public static void InsertOptions(HtmlNode answers, String pageid, String labelanswerid,int formid)
        {
            Option optiondata = new Option();
            HtmlNodeCollection children = answers.ChildNodes;
            int count = 0;
            int flag = 0;
            foreach (var n in children)
            {
                if (n.NodeType == HtmlNodeType.Element)
                {
                    if (flag == 0)// To remove the tag please make a selection
                        flag++;
                    else
                    {
                        optiondata.Option_text = n.InnerText;
                        optiondata.Page_id = pageid;
                        optiondata.Label_answer_id = labelanswerid;
                        optiondata.Formid = formid;
                        optiondata.Option_value = n.Attributes["value"].Value;
                        Option checkoption = DapperORM.CheckAddOption(optiondata);
                        System.Diagnostics.Debug.WriteLine("Check Option" + checkoption);
                        if (checkoption != null)
                        {
                            System.Diagnostics.Debug.WriteLine("Tuple already exists");
                            System.Diagnostics.Debug.WriteLine(labelanswerid);
                        }
                        else
                        {

                            //calling the procedure
                            if (DapperORM.AddOption(optiondata) > 0)
                            {
                                System.Diagnostics.Debug.WriteLine("Row option inserted");
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("Error");

                            }

                            count++;// Count of no of Options


                        }

                    }


                }


            }

        }

    }
}
   

