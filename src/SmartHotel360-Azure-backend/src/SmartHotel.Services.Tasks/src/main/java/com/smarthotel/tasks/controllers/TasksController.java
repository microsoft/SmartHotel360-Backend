package com.smarthotel.tasks.controllers;

import com.smarthotel.tasks.models.Task;
import com.smarthotel.tasks.repositories.TaskRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Sort;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.Date;

@RestController
public class TasksController {

    @Autowired
    private TaskRepository taskRepository;

    @GetMapping("/tasks")
    public @ResponseBody
    Iterable<Task> getAll() {
        return taskRepository.findAll(new Sort(Sort.Direction.DESC, "submitted"));
    }

    @GetMapping("/tasks/pending")
    public @ResponseBody
    Iterable<Task> getPending() {
        return taskRepository.findByResolved(false, new Sort(Sort.Direction.DESC, "submitted"));
    }

    @PutMapping("/tasks/resolved/{id}")
    public @ResponseBody
    ResponseEntity resolveTask(@PathVariable int id) {
        Task task = taskRepository.findOne(id);

        if(task == null){
            return new ResponseEntity(HttpStatus.NOT_FOUND);
        }

        task.setResolved(true);
        taskRepository.save(task);
        return  new ResponseEntity(task, HttpStatus.OK);
    }

    @PutMapping("/tasks/pending/{id}")
    public @ResponseBody
    ResponseEntity unresolveTask(@PathVariable int id) {
        Task task = taskRepository.findOne(id);

        if(task == null){
            return new ResponseEntity(HttpStatus.NOT_FOUND);
        }

        task.setResolved(false);
        taskRepository.save(task);
        return new ResponseEntity(task, HttpStatus.OK);
    }


    @PostMapping("/tasks/{id}")
    public @ResponseBody
    ResponseEntity changeStatus(@PathVariable int id, @RequestBody Task newData) {
        Task task = taskRepository.findOne(id);

        if(task != null){
            return new ResponseEntity(HttpStatus.BAD_REQUEST);
        }

        task.setResolved(newData.getResolved());

        taskRepository.save(task);

        return new ResponseEntity(task, HttpStatus.OK);
    }
}
