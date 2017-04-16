using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;


namespace VKParallelCatcher
{
    class Request
    {
        public static string getUsers(List<String> piece_of_users_list, string proxy_string)
        {
            string MyProxyHostString = proxy_string.Substring(0, proxy_string.IndexOf(":"));
            int MyProxyPort = Int32.Parse(proxy_string.Substring(proxy_string.IndexOf(":") + 1));

            string users = "";
            for (int i = 0; i < piece_of_users_list.Count - 1; i++)
            {
                users += piece_of_users_list[i] + ",";
            }
            users += piece_of_users_list[piece_of_users_list.Count - 1];


            string fieldsLine = "sex,bdate,city,country,has_photo,site,education,followers_count,relation,personal,career,domain";

            string request_string = "https://api.vk.com/method/users.get?user_ids=" + users + "&fields=" + fieldsLine + "&v=5.633";
            Console.WriteLine("USERS FOR PROXY {1} : {0}", users, proxy_string);

            int reqCount = 0; //how many attempts
            string result = "";
            while (true)
            {
            try
            {

                reqCount += 1;
                HttpWebRequest request = WebRequest.Create(request_string) as HttpWebRequest;
                request.Proxy = new WebProxy(MyProxyHostString, MyProxyPort);

                WebResponse response = request.GetResponse();

                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string line = "";
                        while ((line = reader.ReadLine()) != null)
                        {
                            result += line;
                            Console.WriteLine(line);
                        }
                    }
                }
                response.Close();
                Console.WriteLine("Request made with proxy {0} : {1}", MyProxyHostString, MyProxyPort);
                return result;

            }
            catch (WebException)
            {
                if (reqCount <= 5)
                    Console.WriteLine("Proxy {0} having problems, next try...", proxy_string);
                else
                {
                    Console.WriteLine("Proxy {0} not responding", proxy_string);
                    return "";
                }
            }
            }


        }

        public static bool isProxyValid(string proxy_string)
        {
            string MyProxyHostString = proxy_string.Substring(0, proxy_string.IndexOf(":"));
            int MyProxyPort = Int32.Parse(proxy_string.Substring(proxy_string.IndexOf(":") + 1));

            string request_string = "https://api.vk.com/method/users.get?user_ids";

            try
            {


                HttpWebRequest request = WebRequest.Create(request_string) as HttpWebRequest;
                request.Proxy = new WebProxy(MyProxyHostString, MyProxyPort);

                WebResponse response = request.GetResponse();

      
                response.Close();
                Console.WriteLine("Proxy {0} is OK!", proxy_string);
                return true;

            }
            catch (WebException)
            {
                Console.WriteLine("Proxy {0} is not working!", proxy_string);
                return false;
            }
        }

        public static string getTopUserSubs(int user_id, string proxy_string)
        {
            string MyProxyHostString = proxy_string.Substring(0, proxy_string.IndexOf(":"));
            int MyProxyPort = Int32.Parse(proxy_string.Substring(proxy_string.IndexOf(":") + 1));  


            string request_string = "https://api.vk.com/method/users.getSubscriptions?user_id=" + user_id.ToString() + "&extended=1&count=5&v=5.63";
            Console.WriteLine("USER FOR PROXY {1} : {0}", user_id, proxy_string);

            int reqCount = 0; //how many attempts
            string result = "";
            while (true)
            {
                try
                {

                    reqCount += 1;
                    HttpWebRequest request = WebRequest.Create(request_string) as HttpWebRequest;
                    request.Proxy = new WebProxy(MyProxyHostString, MyProxyPort);

                    WebResponse response = request.GetResponse();

                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string line = "";
                            while ((line = reader.ReadLine()) != null)
                            {
                                result += line;
                           //     Console.WriteLine(line);
                            }
                        }
                    }
                    response.Close();
                    Console.WriteLine("Request made with proxy {0} : {1}", MyProxyHostString, MyProxyPort);
                    return result;

                }
                catch (WebException)
                {
                    if (reqCount <= 5)
                        Console.WriteLine("Proxy {0} having problems, next try...", proxy_string);
                    else
                    {
                        Console.WriteLine("Proxy {0} not responding", proxy_string);
                        return "";
                    }
                }
            }
        }

        public static string getCountPhotos(int user_id, string proxy_string)
        {
            string MyProxyHostString = proxy_string.Substring(0, proxy_string.IndexOf(":"));
            int MyProxyPort = Int32.Parse(proxy_string.Substring(proxy_string.IndexOf(":") + 1));


            string request_string = "https://api.vk.com/method/photos.getAlbums?owner_id=" + user_id.ToString() + "&need_system=1&v=5.63";
            Console.WriteLine("USER FOR PROXY {1} : {0}", user_id, proxy_string);

            int reqCount = 0; //how many attempts
            string result = "";
            while (true)
            {
                try
                {

                    reqCount += 1;
                    HttpWebRequest request = WebRequest.Create(request_string) as HttpWebRequest;
                    request.Proxy = new WebProxy(MyProxyHostString, MyProxyPort);

                    WebResponse response = request.GetResponse();

                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string line = "";
                            while ((line = reader.ReadLine()) != null)
                            {
                                result += line;
                                //     Console.WriteLine(line);
                            }
                        }
                    }
                    response.Close();
                    Console.WriteLine("Request made with proxy {0} : {1}", MyProxyHostString, MyProxyPort);
                    return result;

                }
                catch (WebException)
                {
                    if (reqCount <= 5)
                        Console.WriteLine("Proxy {0} having problems, next try...", proxy_string);
                    else
                    {
                        Console.WriteLine("Proxy {0} not responding", proxy_string);
                        return "";
                    }
                }
            }
        }

    }
}
