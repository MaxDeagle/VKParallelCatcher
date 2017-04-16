using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class User
{
    public int id;

    public int hidden;
    public string first_name;
    public string last_name;
    public string bdate;
    public List<string> career_ids;
    public int city_id;
    public int country_id;
    public int university_id;
    public int followers_count;
    public bool has_photo;
    public int political;
    public int people_main;
    public int life_main;
    public int smoking;
    public int alcohol;
    public int relation;
    public int sex;
    public string site;
    public List<string> top_five_subs;
    public int count_main_photos;
    public string domain;
    public int same_points;


    public User()
    {
        this.id = -1;
        this.hidden = -1;
        this.first_name = "";
        this.last_name = "";
        this.bdate = "";
        this.career_ids = new List<string>();
        this.city_id = -1;
        this.country_id = -1;
        this.university_id = -1;
        this.followers_count = -1;
        this.has_photo = false;
        this.political = -1;
        this.people_main = -1;
        this.life_main = -1;
        this.smoking = -1;
        this.alcohol = -1;
        this.relation = -1;
        this.sex = -1;
        this.site = "";
        this.top_five_subs = new List<string>();
        this.count_main_photos = -1;
        this.domain = "";
        this.same_points = 0;
    }
}