import { Component, OnInit } from '@angular/core';
import { Post } from "../../models/post";
import { PostsService } from "../../services/posts/posts.service";
import { Router, ActivatedRoute } from "@angular/router";

import { FormBuilder, FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrls: ['./create-post.component.css']
})
export class CreatePostComponent implements OnInit {

  post: Post = new Post;
  tagField: string;
  constructor(private route: ActivatedRoute,
    private postService: PostsService) { }

  onTagFieldInput(tag: string) {
    this.tagField = tag;
  }
  addTag() {
    const tag = this.tagField;
    if (this.post.Tags.indexOf(tag) < 0) {
      this.post.Tags.push(tag);
    }
    this.tagField = "";
  }

  removeTag(tagName: string) {
    this.post.Tags = this.post.Tags.filter(tag => tag !== tagName);
  }
  addPost() {
    if (this.post.Id) {
      this.post.IsApproved = false;
      this.postService.updatePost(this.post).subscribe();
    }
    else {
      this.postService.addPost(this.post).subscribe();
    }
  }
  ngOnInit() {
    const id = +this.route.snapshot.paramMap.get('id');

    if (id == 0) {
      this.post = new Post();
      this.post.Tags = [];
    }
    else {
      this.postService.getPost(id).subscribe(
        p => {
          this.post = p;
        });

    }
  }
}
