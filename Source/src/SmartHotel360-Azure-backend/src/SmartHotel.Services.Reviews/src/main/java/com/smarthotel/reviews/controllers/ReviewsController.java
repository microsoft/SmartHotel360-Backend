package com.smarthotel.reviews.controllers;


import com.smarthotel.reviews.models.Review;
import com.smarthotel.reviews.repositories.ReviewRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Sort;
import org.springframework.web.bind.annotation.*;

@RestController
public class ReviewsController {

    @Autowired
    private ReviewRepository reviewRepository;

    @CrossOrigin()
    @GetMapping("/reviews/hotel/{id}")
    public @ResponseBody
    Iterable<Review> getByHotel(@PathVariable int id) {
        return reviewRepository.findByHotelId(id, new Sort(Sort.Direction.DESC, "submitted"));
    }
}
