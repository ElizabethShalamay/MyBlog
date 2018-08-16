import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from "@angular/router";
import { AccountService } from "../../services/account/account.service";
import { Post } from "../../models/post";
import { PostsService } from '../../services/posts/posts.service';
import { CommentsService } from '../../services/comments/comments.service';


@Component({
  selector: 'app-single-post',
  templateUrl: './single-post.component.html',
  styleUrls: ['./single-post.component.css']
})
export class SinglePostComponent implements OnInit {

  post: Post;
  postId: number;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private accService: AccountService,
    private postsService: PostsService,
    private commentsService: CommentsService) { }

  getPost(): void {
    const id = +this.route.snapshot.paramMap.get('id');
    this.postsService.getPost(id)
      .subscribe(
        post => {
          this.post = post;
          this.postId = post.Id;
        });
  }

  search(search: string) {
    this.postsService.search(`#${search}`).subscribe(data =>
      this.postsService.posts.push(...data)
    );
    this.router.navigate(['search']);
  }

  ngOnInit() {
    this.getPost();
  }
}
