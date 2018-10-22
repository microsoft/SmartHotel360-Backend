package com.smarthotel.tasks.repositories;

import com.smarthotel.tasks.models.Task;
import org.springframework.data.domain.Sort;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.repository.CrudRepository;

import java.util.List;

public interface TaskRepository extends JpaRepository<Task, Integer> {

    List<Task> findByResolved(boolean resolved, Sort sort);
}
