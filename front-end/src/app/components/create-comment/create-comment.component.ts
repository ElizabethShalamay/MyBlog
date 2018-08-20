import { Component, OnInit, Input } from '@angular/core';
import { CommentsService } from '../../services/comments/comments.service';
import { Comment } from "../../models/comment";
import { ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-create-comment',
  templateUrl: './create-comment.component.html',
  styleUrls: ['./create-comment.component.css']
})
export class CreateCommentComponent implements OnInit {

  @Input() parentId: number = 0;
  @Input() comment: Comment = new Comment();
  text: string;

  constructor(private commentsService: CommentsService,
    private route: ActivatedRoute) { }

  onCommentInput(value: string) {
    this.text = value;
  }

  addComment() {
    if (this.comment.Id) {
      this.comment.IsApproved = false;
      this.commentsService.updateComment(this.comment).subscribe();
    }
    else {
      const id = +this.route.snapshot.paramMap.get('id');
      this.commentsService.addComment(id, this.text, this.parentId).subscribe();
    }
    this.comment.Text = "";
  }

  ngOnInit() {
    if (this.comment) {
      this.text = this.comment.Text;
    }
  }
}
