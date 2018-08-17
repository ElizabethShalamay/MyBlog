import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { Comment } from "../../models/comment";
import { CommentsService } from "../../services/comments/comments.service";
import { Router, ActivatedRoute } from "@angular/router";

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.css']
})
export class CommentsComponent implements OnInit {

  @Input() comments: Comment[] = [];
  seed: number;
  page = 1;

  constructor(private commentsService: CommentsService,
    private route: ActivatedRoute) { }

  getComments() { // todo: add pagination
    if(!this.comments.length)
      {
        const id = +this.route.snapshot.paramMap.get('id');
        this.commentsService.getComments(id, this.page).subscribe(
        data => {
          this.comments.push(...data);
          console.log(data);
          this.page++;
        }
      );
    }
  }

  ngOnInit() {
    this.getComments();
  }
}
