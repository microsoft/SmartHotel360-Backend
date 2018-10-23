package com.smarthotel.tasks.seeders;

import com.smarthotel.tasks.models.Task;
import com.smarthotel.tasks.repositories.TaskRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.event.ContextRefreshedEvent;
import org.springframework.context.event.EventListener;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Component;

import java.util.*;
import java.util.concurrent.ThreadLocalRandom;

@Component
public class TaskSeeder {

    private JdbcTemplate jdbcTemplate;
    private TaskRepository taskRepository;


    @Autowired
    public TaskSeeder(TaskRepository taskRepository,
                      JdbcTemplate jdbcTemplate) {
        this.taskRepository = taskRepository;
        this.jdbcTemplate = jdbcTemplate;
    }

    @EventListener
    public void seed(ContextRefreshedEvent event) {
        boolean alreadySeeded = taskRepository.count() > 0;

        if (alreadySeeded) {
            return;
        }

        List<String> statuses = new ArrayList<String>();
        statuses.add("pending");
        statuses.add("resolved");

        Map<Integer, String> taskTypes = new HashMap<>();
        taskTypes.put(5, "changeTowels");
        taskTypes.put(2, "cleanRoom");
        taskTypes.put(3, "newGuest");
        taskTypes.put(4, "roomService");
        taskTypes.put(1, "airConditioner");

        List<TaskAndType>  tasks = new ArrayList<>();
        tasks.add(new TaskAndType(1,"AC is stuck on, I have played with the dial and no matter what I do it keeps blowing cold air."));
        tasks.add(new TaskAndType(1,"My AC unit has a weird smell coming from it. Smells like mold."));
        tasks.add(new TaskAndType(1,"The AC in my room is very loud, sounds like a lawn mower is running in my room."));
        tasks.add(new TaskAndType(1, "Air conditioning is not working, no power, no fan... Nothing."));
        tasks.add(new TaskAndType(4, "Room service forgot my drinks, I ordered a fizzy drink and grape juice"));
        tasks.add(new TaskAndType(4, "I ordered my breakfast over an hour ago... where is my pancake...????"));
        tasks.add(new TaskAndType(4, "My kids ordered a bunch of food through the TV, and I need to cancel my room orders. ASAP"));
        tasks.add(new TaskAndType(3, "I lost my room key already, I need a replacement key"));
        tasks.add(new TaskAndType(3, "I want a west facing window, very important for my sleep cycle, please move me."));
        tasks.add(new TaskAndType(3, "Can I get a daily newspaper on my door each morning?"));
        tasks.add(new TaskAndType(3, "Deliver a fold out bed for one of my kids."));
        tasks.add(new TaskAndType(5, "Please come by my room this afternoon and replace towels and mats"));
        tasks.add(new TaskAndType(2, "Vacuum room and wipe down the counters, it seems dusty in here"));
        tasks.add(new TaskAndType(2, "Sterilize the room with bleach please, I am germ-phobic..."));
        tasks.add(new TaskAndType(2, "Clean toilet, shower, and sink"));

        for (int i = 0; i < tasks.size(); i++) {
            Task task = new Task();
            task.setUserId("bwaiters@smarthotel360.com");
            task.setRoom((i+10) * 4);
            TaskAndType ttype = tasks.get(i);
            task.setTaskType(ttype.type );
            task.setResolved(false);
            task.setDescription(ttype.task);
            taskRepository.save(task);
        }
    }
}

class TaskAndType
{
    public String task;
    public int type;

    public TaskAndType(int type, String task) {
        this.task = task;
        this.type =type;
    }
}
