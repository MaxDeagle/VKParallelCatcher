using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using Newtonsoft.Json.Linq;

namespace VKParallelCatcher
{
    class Program
    {
 

 



        static void Main(string[] args)
        {

            String TARGET_ACC = System.IO.File.ReadAllLines("target.txt")[0];
            //read acc_list
            List<String> ACC_LIST = new List<String>(System.IO.File.ReadAllLines("acclist.txt").Distinct().ToArray());
            //read proxy
            List<String> PROXY_LIST = new List<String> (System.IO.File.ReadAllLines("proxylist.txt").Distinct().ToArray());

            //check proxys
            var threadsCheck = new List<Thread>();
            int proxy_iterator = 0;
            while (proxy_iterator < PROXY_LIST.Count)
            {
                string temp_proxy = PROXY_LIST[proxy_iterator];
                Thread proxy_check_thread = new Thread(() =>
                {
                    if (!Thread_function_check_proxy(temp_proxy))
                    {
                        PROXY_LIST.Remove(temp_proxy);
                        proxy_iterator--;
                    }
                });
                proxy_check_thread.Start();
                //Thread.Sleep(10);
                threadsCheck.Add(proxy_check_thread);
      
                proxy_iterator++;
            }

            foreach (var thread in threadsCheck)
                thread.Join();

            for (int i = 0; i < PROXY_LIST.Count; i++)
            {
                Console.WriteLine(PROXY_LIST[i]);
            }

            if (PROXY_LIST.Count >= ACC_LIST.Count) //if 1 proxy for 1 acc
            {

                PROXY_LIST.RemoveRange(0, PROXY_LIST.Count - ACC_LIST.Count);
             }

            int k = 0;
            List<List<String>> list_of_proxy = new List<List<String>>(); //each element contains users for current proxy

            for (int i = 0; i < PROXY_LIST.Count; i++)
            {
                list_of_proxy.Add(new List<String>());
            }
            for (int i = 0; i < ACC_LIST.Count; i++)
            {

                list_of_proxy[k].Add(ACC_LIST[i]);
                k++;
                if (k == PROXY_LIST.Count)
                    k = 0;
      

            }
            list_of_proxy[list_of_proxy.Count - 1].Add(TARGET_ACC); //add target user


 

            List<dynamic> json_objects_to_handle = new List<dynamic>();


            //Start collectioning data
            var threads = new List<Thread>();
            for (int thread_number = 0; thread_number < PROXY_LIST.Count; thread_number++)
            {
                List<String> temp_list = new List<String>();
                for (int i = 0; i < list_of_proxy[thread_number].Count; i++)
                    temp_list.Add(list_of_proxy[thread_number][i]);

                string temp_proxy = PROXY_LIST[thread_number];

                Thread proxy_thread = new Thread(() => { json_objects_to_handle.Add(JObject.Parse(Thread_function(temp_list, temp_proxy))); });
                
                proxy_thread.Name = "thread " + PROXY_LIST[0].ToString();
                proxy_thread.Start();
                //Thread.Sleep(10);
                threads.Add(proxy_thread);
            }
        //    Console.ReadKey();

            foreach (var thread in threads)
                thread.Join();


            List<User> users_with_data = new List<User>();
            User target_user = new User();
            int count_users = 0;

            //parsing data
            while (count_users <= ACC_LIST.Count)
            for (int i = 0; i < json_objects_to_handle.Count; i++)
            {
                for (int j = 0; j < json_objects_to_handle[i].response.Count; j++)
                {                    
                    User temp_user = new User();
                    Console.WriteLine(json_objects_to_handle[i].response[j].GetValue("id"));
                    temp_user.id = Int32.Parse(json_objects_to_handle[i].response[j].GetValue("id").ToString());
                    temp_user.first_name = json_objects_to_handle[i].response[j].GetValue("first_name");

                    if (json_objects_to_handle[i].response[j].Property("last_name") != null)
                        temp_user.last_name = json_objects_to_handle[i].response[j].GetValue("last_name");
                    if (json_objects_to_handle[i].response[j].Property("sex") != null)
                        temp_user.sex = Int32.Parse(json_objects_to_handle[i].response[j].GetValue("sex").ToString());
                    if (json_objects_to_handle[i].response[j].Property("bdate") != null)
                        temp_user.bdate = json_objects_to_handle[i].response[j].GetValue("bdate");
                    if (json_objects_to_handle[i].response[j].Property("city") != null)
                        temp_user.city_id = Int32.Parse(json_objects_to_handle[i].response[j].city.GetValue("id").ToString());
                    if (json_objects_to_handle[i].response[j].Property("country") != null)
                        temp_user.country_id = Int32.Parse(json_objects_to_handle[i].response[j].country.GetValue("id").ToString());
                    int has_photo = Int32.Parse(json_objects_to_handle[i].response[j].GetValue("has_photo").ToString());
                    temp_user.has_photo = (has_photo == 1);
                    if (json_objects_to_handle[i].response[j].Property("site") != null)
                        temp_user.site = json_objects_to_handle[i].response[j].GetValue("site");
                    if (json_objects_to_handle[i].response[j].Property("followers_count") != null)
                        temp_user.site = json_objects_to_handle[i].response[j].GetValue("followers_count");
                    if (json_objects_to_handle[i].response[j].Property("career") != null)
                        for (k = 0; k < json_objects_to_handle[i].response[j].career.Count; k++)
                        {
                            temp_user.career_ids.Add(json_objects_to_handle[i].response[j].career[k].GetValue("id").ToString());
                        }
                    if (json_objects_to_handle[i].response[j].Property("university") != null)
                        temp_user.university_id = Int32.Parse(json_objects_to_handle[i].response[j].GetValue("university").ToString());
                    if (json_objects_to_handle[i].response[j].Property("relation") != null)
                        temp_user.relation = Int32.Parse(json_objects_to_handle[i].response[j].GetValue("relation").ToString());
                    if (json_objects_to_handle[i].response[j].Property("personal") != null)
                    {
                        if (json_objects_to_handle[i].response[j].personal.Property("political") != null)
                            temp_user.political = Int32.Parse(json_objects_to_handle[i].response[j].personal.GetValue("political").ToString());
                        if (json_objects_to_handle[i].response[j].personal.Property("people_main") != null)
                            temp_user.people_main = Int32.Parse(json_objects_to_handle[i].response[j].personal.GetValue("people_main").ToString());
                        if (json_objects_to_handle[i].response[j].personal.Property("life_main") != null)
                            temp_user.life_main = Int32.Parse(json_objects_to_handle[i].response[j].personal.GetValue("life_main").ToString());
                        if (json_objects_to_handle[i].response[j].personal.Property("smoking") != null)
                            temp_user.smoking = Int32.Parse(json_objects_to_handle[i].response[j].personal.GetValue("smoking").ToString());
                        if (json_objects_to_handle[i].response[j].personal.Property("alcohol") != null)
                            temp_user.alcohol = Int32.Parse(json_objects_to_handle[i].response[j].personal.GetValue("alcohol").ToString());
                    }
                    if (json_objects_to_handle[i].response[j].Property("hidden") != null)
                        temp_user.hidden = Int32.Parse(json_objects_to_handle[i].response[j].GetValue("hidden").ToString());
                    if (json_objects_to_handle[i].response[j].Property("domain") != null)
                        temp_user.domain = json_objects_to_handle[i].response[j].GetValue("domain").ToString();

                    if (temp_user.domain == TARGET_ACC)
                        target_user = temp_user;
                    else
                        users_with_data.Add(temp_user);
                    count_users++;
                }
            }

            for (int i = 0; i < users_with_data.Count; i++)
                Console.WriteLine("{0} {1}", users_with_data[i].first_name, users_with_data[i].last_name);
            Console.WriteLine("AND {0} {1}", target_user.first_name, target_user.last_name);
        //    Console.ReadKey();

            //getting top subs
            int proxy_number = 0;
            var threads_users = new List<Thread>();
            for (int i = 0; i < users_with_data.Count; i++)
            {
                 User temp_user = users_with_data[i];
        
                 string temp_proxy = PROXY_LIST[proxy_number];

                 Thread user_thread = new Thread(() => {
                     dynamic json_object_to_handle = JObject.Parse(Thread_function_get_subs(temp_user.id, temp_proxy));
               //      Console.WriteLine(json_object_to_handle);
                     if (json_object_to_handle.response.Property("items") != null)
                         for (int j = 0; j < json_object_to_handle.response.items.Count; j++)
                        {
                  //          Console.WriteLine(json_object_to_handle.response.items[j].GetValue("id"));
                            temp_user.top_five_subs.Add(json_object_to_handle.response.items[j].GetValue("id").ToString());
                        }
                 });

                 user_thread.Name = "thread " + PROXY_LIST[0].ToString();
                 user_thread.Start();

                 users_with_data[i] = temp_user;
                 //Thread.Sleep(10);
                 threads_users.Add(user_thread);

                 
                 proxy_number++;
                 if (proxy_number == PROXY_LIST.Count)
                     proxy_number = 0;
 
            }
            foreach (var thread in threads_users)
                thread.Join();

            dynamic json_object_to_handle_target = JObject.Parse(Request.getTopUserSubs(target_user.id, PROXY_LIST[0]));
            //      Console.WriteLine(json_object_to_handle);
            if (json_object_to_handle_target.response.Property("items") != null)
                for (int j = 0; j < json_object_to_handle_target.response.items.Count; j++)
                {
                    //          Console.WriteLine(json_object_to_handle.response.items[j].GetValue("id"));
                    target_user.top_five_subs.Add(json_object_to_handle_target.response.items[j].GetValue("id").ToString());
                }



            //getting count of main photos
            proxy_number = 0;
            threads_users = new List<Thread>();
            for (int i = 0; i < users_with_data.Count; i++)
            {
                User temp_user = users_with_data[i];

                string temp_proxy = PROXY_LIST[proxy_number];

                Thread user_thread = new Thread(() =>
                {
                    dynamic json_object_to_handle = JObject.Parse(Thread_function_get_count_photos(temp_user.id, temp_proxy));
                       //   Console.WriteLine(json_object_to_handle);
                    if (json_object_to_handle.response.Property("items") != null)
                        for (int j = 0; j < json_object_to_handle.response.items.Count; j++)
                        {
                            if (json_object_to_handle.response.items[j].GetValue("id").ToString().Equals("-6"))
                            temp_user.count_main_photos = Int32.Parse(json_object_to_handle.response.items[j].GetValue("size").ToString());
                        }
                });

                user_thread.Name = "thread " + PROXY_LIST[0].ToString();
                user_thread.Start();

                users_with_data[i] = temp_user;
                //Thread.Sleep(10);
                threads_users.Add(user_thread);


                proxy_number++;
                if (proxy_number == PROXY_LIST.Count)
                    proxy_number = 0;

            }
            foreach (var thread in threads_users)
                thread.Join();

            json_object_to_handle_target = JObject.Parse(Request.getCountPhotos(target_user.id, PROXY_LIST[0]));
            //      Console.WriteLine(json_object_to_handle);
            if (json_object_to_handle_target.response.Property("items") != null)
                for (int j = 0; j < json_object_to_handle_target.response.items.Count; j++)
                {
                    if (json_object_to_handle_target.response.items[j].GetValue("id").ToString().Equals("-6"))
                        target_user.count_main_photos = Int32.Parse(json_object_to_handle_target.response.items[j].GetValue("size").ToString());
                }

            for (int i = 0; i < users_with_data.Count; i++)
            {
                Console.WriteLine("{0} {1} {2}", users_with_data[i].first_name, users_with_data[i].last_name, users_with_data[i].count_main_photos);
                for (int j = 0; j < users_with_data[i].top_five_subs.Count; j++)
                    Console.WriteLine(users_with_data[i].top_five_subs[j]);
            }
          //  Console.ReadKey();
            Console.WriteLine("{0} {1} {2} {3}", target_user.first_name, target_user.last_name, target_user.count_main_photos, target_user.top_five_subs[0]);
          //  Console.ReadKey();


            for (int i = 0; i < users_with_data.Count; i++)
            {
                if (users_with_data[i].alcohol == target_user.alcohol) users_with_data[i].same_points += 1;
                if (users_with_data[i].political == target_user.political) users_with_data[i].same_points += 1;
                if (users_with_data[i].relation == target_user.relation) users_with_data[i].same_points += 1;
                if (users_with_data[i].sex == target_user.sex) users_with_data[i].same_points += 1;
                if (users_with_data[i].site.Equals(target_user.site)) users_with_data[i].same_points += 1;
                if (users_with_data[i].smoking == target_user.smoking) users_with_data[i].same_points += 1;
                if (users_with_data[i].university_id == target_user.university_id) users_with_data[i].same_points += 1;
                if (users_with_data[i].city_id == target_user.city_id) users_with_data[i].same_points += 1;
                if ((users_with_data[i].count_main_photos > target_user.count_main_photos - 5) && users_with_data[i].count_main_photos < target_user.count_main_photos + 5) users_with_data[i].same_points += 1;
                if (users_with_data[i].country_id == target_user.country_id) users_with_data[i].same_points += 1;
                if ((users_with_data[i].followers_count > target_user.followers_count - 100) && users_with_data[i].followers_count < target_user.followers_count + 100) users_with_data[i].same_points += 1;
                if (users_with_data[i].has_photo == target_user.has_photo) users_with_data[i].same_points += 1;
                if (users_with_data[i].life_main == target_user.life_main) users_with_data[i].same_points += 1;
                if (users_with_data[i].people_main == target_user.people_main) users_with_data[i].same_points += 1;

                var result = users_with_data[i].top_five_subs.Intersect(target_user.top_five_subs);
                foreach (string s in result)
                    users_with_data[i].same_points += 1;

                result = users_with_data[i].career_ids.Intersect(target_user.career_ids);
                foreach (string s in result)
                    users_with_data[i].same_points += 1;
                


            }

            users_with_data.Sort((x, y) =>
            y.same_points.CompareTo(x.same_points));

            for (int i = 0; i < users_with_data.Count; i++)
                Console.WriteLine("{0} {1}", users_with_data[i].first_name, users_with_data[i].last_name);
            Console.ReadKey();
        }


    
            static string Thread_function(List<String> piece_of_users_list, string proxy_string)
            {
                return Request.getUsers(piece_of_users_list, proxy_string);
            }

            static bool Thread_function_check_proxy(string proxy_string)
            {
                return Request.isProxyValid(proxy_string);
            }

            static string Thread_function_get_subs(int user_id, string proxy_string)
            {
                return Request.getTopUserSubs(user_id, proxy_string);
            }

            static string Thread_function_get_count_photos(int user_id, string proxy_string)
            {
                return Request.getCountPhotos(user_id, proxy_string);
            }
    }
}
