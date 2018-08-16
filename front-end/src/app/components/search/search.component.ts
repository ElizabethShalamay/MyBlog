import { Component,OnInit, AfterViewChecked } from '@angular/core';
import { Post } from "../../models/post";
import { PostsService } from '../../services/posts/posts.service';
@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit, AfterViewChecked {
  posts: Post[] = [];
  page = 1;

  constructor(
    private postsService: PostsService
  ) { }

  ngOnInit() {
    this.posts = this.postsService.posts;
  }
  ngAfterViewChecked() {
    if (this.posts != this.postsService.posts)
      this.posts = this.postsService.posts;
      console.log(this.postsService.posts);     

  }
}
