import { Component, OnInit, Input } from '@angular/core';
import { CommentsService } from '../../services/comments/comments.service'
import { Comment } from "../../models/comment";

@Component({
  selector: 'app-single-comment',
  templateUrl: './single-comment.component.html',
  styleUrls: ['./single-comment.component.css']
})
export class SingleCommentComponent implements OnInit {

  @Input() comment:Comment;
  answer: boolean = false;

  constructor(private commentsService: CommentsService) { }

  deleteComment(){
    this.commentsService.removeComment(this.comment).subscribe();
  }

  answerComment(){
    this.answer = !this.answer;
  }

  ngOnInit() {
  }
}
