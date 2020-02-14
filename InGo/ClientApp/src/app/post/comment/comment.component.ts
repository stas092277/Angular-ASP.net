import { Component, OnInit, Input, Output, EventEmitter  } from '@angular/core';
import { CommentService } from './comment.service';
import { comment} from './Itcomment';
import { ActivatedRoute } from '@angular/router';
import { AccountService } from 'src/app/start-page/account.service';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.sass']
})
export class CommentComponent implements OnInit {

  @Input() commentList: comment[];

  // commentList: comment[] = [];
  defaultValue = '';
  userDetails;

  constructor(
    private _commentService: CommentService,
    private accountService: AccountService,
    private route: ActivatedRoute
    ) { }

  ngOnInit() {
    // const id = +this.route.snapshot.paramMap.get('id');
    // this._commentService.getDataComment(id).subscribe(data => this.commentList = data.,
    //   err => console.log('HTTP error', err.message));
    console.log(this.commentList)
    this.accountService.GetUserProfile().subscribe(res => {
      this.userDetails = res;
}, err => { console.log(err); }, ); }

receiveComment($event){
  this.commentList.push($event);
}

}

