using System;

public class User
{
    public int id;

    public int hidden;
    public string first_name;
    public string last_name;
    public string bdate;
    public int career_id;
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
    public string[] top_five_subs;
    public int count_main_photos;


    public User()
    {
        this.id = -1;
        this.hidden = -1;
        this.first_name = "";
        this.last_name = "";
        this.bdate = "";
        this.career_id = -1;
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
        this.top_five_subs = new string[5];
        this.count_main_photos = -1;
    }
}