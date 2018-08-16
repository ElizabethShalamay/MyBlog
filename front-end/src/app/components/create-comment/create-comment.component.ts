import { Component, OnInit, Input } from '@angular/core';
import { CommentsService } from '../../services/comments/comments.service';
import { Comment } from "../../models/comment";
import { ActivatedRoute, Router } from "@angular/router";
import { Location } from "@angular/common";

@Component({
  selector: 'app-create-comment',
  templateUrl: './create-comment.component.html',
  styleUrls: ['./create-comment.component.css']
})
export class CreateCommentComponent implements OnInit {

  @Input() parentId: number = 0;
  text: string;
  constructor(private commentsService: CommentsService,
    private location: Location,
    private route: ActivatedRoute,
    private router: Router) { }

  onCommentInput(value: string) {
    this.text = value;
  }

  addComment() {
    const id = +this.route.snapshot.paramMap.get('id');
    this.commentsService.addComment(id, this.text, this.parentId).subscribe();
  }

  ngOnInit() {
  }
}
