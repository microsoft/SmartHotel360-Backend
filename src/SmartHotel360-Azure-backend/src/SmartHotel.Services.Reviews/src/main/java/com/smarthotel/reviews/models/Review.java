package com.smarthotel.reviews.models;

import com.fasterxml.jackson.annotation.JsonFormat;
import com.fasterxml.jackson.annotation.JsonGetter;
import com.fasterxml.jackson.annotation.JsonInclude;
import com.fasterxml.jackson.annotation.JsonProperty;

import javax.persistence.*;
import java.text.SimpleDateFormat;
import java.util.Date;


@Entity
public class Review {
    @Id
    @GeneratedValue(strategy= GenerationType.AUTO)
    private Integer id;

    private String userId;

    private Date submitted;

    @Column(length = 1024)
    private String description;

    private Integer hotelId;

    private String userName;


    public Integer getId() {
        return this.id;
    }
    public void setId(Integer id) {
        this.id = id;
    }

    public Date getSubmitted() {return this.submitted;}
    public void setSubmitted(Date date) {this.submitted = date;}

    public Integer getHotelId() {return this.hotelId;}
    public void setHotelId(int hotel) {this.hotelId = hotel;}

    public void setUserId (String userid) {this.userId = userid;}
    public String getUserId() {return this.userId;}

    public void setDescription (String desc) {this.description = desc;}
    public String getDescription() {return this.description;}

    public String getUserName() { return this.userName;}
    public void setUserName(String value) { this.userName = value;}

    @JsonProperty("formattedDate")
    public String getFormattedDate() {
        String format = System.getenv("DATE_FORMAT");
        if (format == null) {
            format = "EEE, d MMM yyyy HH:mm:ss Z";
        }

        SimpleDateFormat sdf = new SimpleDateFormat(format);
        return sdf.format(this.submitted);
    }
}
