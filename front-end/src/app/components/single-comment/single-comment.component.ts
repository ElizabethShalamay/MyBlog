import { Component, OnInit, Input } from '@angular/core';
import { CommentsService } from '../../services/comments/comments.service'
import { Comment } from "../../models/comment";
import { AccountService } from '../../services/account/account.service';

@Component({
  selector: 'app-single-comment',
  templateUrl: './single-comment.component.html',
  styleUrls: ['./single-comment.component.css']
})
export class SingleCommentComponent implements OnInit {

  @Input() comment: Comment;
  answer: boolean = false;
  edit: boolean = false;
  isOwn: boolean;

  constructor(private commentsService: CommentsService,
    private accService: AccountService) { }

  deleteComment() {
    if (this.isOwn) {
      this.commentsService.removeComment(this.comment).subscribe();
    }
  }

  updateComment() {
    if (this.isOwn) {
      this.edit = !this.edit;
    }
  }

  answerComment() {
    if (!this.isOwn)
      this.answer = !this.answer;
  }

  ngOnInit() {
    this.isOwn = this.accService.authentication.userName == this.comment.AuthorName;
  }
}
