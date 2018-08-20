import { Component, OnInit } from '@angular/core';
import { Post } from "../../models/post";
import { PostsService } from "../../services/posts/posts.service";
import { Router, ActivatedRoute } from "@angular/router";
import { AccountService } from '../../services/account/account.service';

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrls: ['./create-post.component.css']
})
export class CreatePostComponent implements OnInit {

  post: Post = new Post;
  tagField: string;

  constructor(private route: ActivatedRoute,
    private postService: PostsService,
    private accService: AccountService,
    private router: Router) { }

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
    if (!this.accService.authentication.isAuth) {
      this.router.navigate(["/login"]);
    }
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
