import { Component, OnInit } from '@angular/core';
import { News } from "../../models/news";
import { Tag } from "../../models/tag";
import { PostsService } from "../../services/posts/posts.service";
import { TagsService } from "../../services/tags/tags.service";
import { Router } from '../../../../node_modules/@angular/router';

@Component({
  selector: 'app-news',
  templateUrl: './news.component.html',
  styleUrls: ['./news.component.css']
})
export class NewsComponent implements OnInit {

  news: News[] = [];
  tags: Tag[] = [];
  constructor(private postsService: PostsService,
    private tagsService: TagsService,
    private router: Router) { }

  getNews() {
    this.postsService.getNews().subscribe(
      news => {
        this.news.push(...news);
      });
  }

  getTags() {
    this.tagsService.getAll().subscribe(
      tags => {
        this.tags.push(...tags);
      }
    )
  }
  onTagClick(tag: string) {
    let tagString = `#${tag}`;
    console.log(tagString);

    this.postsService.search(tagString).subscribe(data =>
      this.postsService.posts.push(...data)
    );
    this.router.navigate(['search']);
  }
  ngOnInit() {
    this.getNews();
    this.getTags();
  }
}
