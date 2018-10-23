package com.smarthotel.tasks.models;

import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import java.util.Date;

@Entity
public class Task {

    public Task(){
        submitted = new Date();
    }

    @Id
    @GeneratedValue(strategy=GenerationType.AUTO)
    private Integer id;

    private String userId;

    private int room;

    private boolean resolved;

    private int taskType;

    private Date submitted;

    private String description;


    public Integer getId() {
        return this.id;
    }
    public void setId(Integer id) {
        this.id = id;
    }

    public void setRoom(Integer room) {this.room = room; }
    public Integer getRoom() {return this.room;}

    public void setUserId (String userid) {this.userId = userid;}
    public String getUserId() {return this.userId;}


    public boolean getResolved() {return this.resolved;}
    public void setResolved(boolean resolved) {
        this.resolved = resolved;
    }

    public int getTaskType() {
        return this.taskType;
    }
    public void setTaskType(int taskType) {
        this.taskType = taskType;
    }

    public void setDescription (String desc) {this.description = desc;}
    public String getDescription() {return this.description;}
}
