import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from "@angular/router";
import { AccountService } from "../../services/account/account.service";
import { Post } from "../../models/post";
import { PostsService } from '../../services/posts/posts.service';

@Component({
  selector: 'app-single-post',
  templateUrl: './single-post.component.html',
  styleUrls: ['./single-post.component.css']
})
export class SinglePostComponent implements OnInit {

  post: Post = new Post();
  postId: number;
  isOwn: boolean;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private accService: AccountService,
    private postsService: PostsService) { }

  getPost(): void {
    const id = +this.route.snapshot.paramMap.get('id');
    this.postsService.getPost(id).subscribe(
      post => {
        this.post = post;
        this.postId = post.Id;
        this.isOwn = this.accService.authentication.userName == this.post.AuthorName;
      });
  }

  deletePost(): void {
    this.postsService.removePost(this.post).subscribe();
  }

  search(search: string) {
    this.postsService.search(`#${search}`).subscribe(data => this.postsService.posts.push(...data));
    this.router.navigate(['search']);
  }

  ngOnInit() {
    if (!this.accService.authentication.isAuth) {
      this.router.navigate(["/login"]);
    }
    this.getPost();
  }
}
