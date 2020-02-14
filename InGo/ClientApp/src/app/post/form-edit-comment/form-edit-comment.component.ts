import { Component, OnInit, Output, Input, EventEmitter } from '@angular/core';
import { EditCommentService } from './edit-comment.service';
import { comment } from '../comment/Itcomment';
import { ActivatedRoute } from '@angular/router';
import {formatDate} from '@angular/common';
import { AccountService } from 'src/app/start-page/account.service';
import { PostService} from '../post.service';
import { ToastrService } from 'ngx-toastr';
import { HttpClient, HttpEventType, HttpErrorResponse } from '@angular/common/http';


@Component({
  selector: 'app-form-edit-comment',
  templateUrl: './form-edit-comment.component.html',
  styleUrls: ['./form-edit-comment.component.sass']
})



export class FormEditCommentComponent implements OnInit {
  currentUser;
  newCommentContent:string;
  currentPostId: number;


  @Output() OutToComment = new EventEmitter();


  constructor(
    private _editCommentService: EditCommentService,
    private route: ActivatedRoute,
    private accountService: AccountService,
    private toastr: ToastrService,
    private post: PostService
    ) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.currentPostId = params['id'];
   }); 
  }

  addComment(){
    if (this.newCommentContent == null || undefined)
    {
      this.toastr.error('Комментарий пустой');
      return;
    }

    this.post.addCommentToPost(this.currentPostId, this.newCommentContent).subscribe((res: any)=> {
      this.toastr.success('Комментарий добавлен');
      this.OutToComment.emit(res);
    }, (errorResponse: HttpErrorResponse) =>{
      this.toastr.error('Ошибка добавления комментария');
    }
    );
  }

}
