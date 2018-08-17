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
  @Input() comment:Comment = new Comment();

  text: string;

  

  constructor(private commentsService: CommentsService,
    private location: Location,
    private route: ActivatedRoute,
    private router: Router) { }

  onCommentInput(value: string) {
    this.text = value;
  }

  addComment() {
    console.log(this.comment.Id);
    if (this.comment.Id) {
      this.comment.IsApproved = false;
      this.commentsService.updateComment(this.comment).subscribe();
    }
    else {
      const id = +this.route.snapshot.paramMap.get('id');
      this.commentsService.addComment(id, this.text, this.parentId).subscribe();    
    }
  }

  ngOnInit() {

    if(this.comment){
      this.text = this.comment.Text;
    }
    // const id = +this.route.snapshot.paramMap.get('id');
    // console.log(id);

    // if (id == 0) {
    //   this.comment = new Comment();
    // }
    // else {
    //   this.commentsService.getComment(id).subscribe(
    //     c => {
    //       this.comment = c;
    //       console.log(c);
    //       console.log(this.comment);
    //     });

    // }
  }
}
