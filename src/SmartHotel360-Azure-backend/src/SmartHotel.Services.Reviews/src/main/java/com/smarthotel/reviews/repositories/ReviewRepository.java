package com.smarthotel.reviews.repositories;

import com.smarthotel.reviews.models.Review;
import org.springframework.data.domain.Sort;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;


public interface ReviewRepository extends JpaRepository<Review, Integer> {
    List<Review> findByHotelId(int hotelId, Sort sort);
}

