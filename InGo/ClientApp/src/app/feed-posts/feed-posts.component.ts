import { Component, OnInit, Input, Output, EventEmitter  } from '@angular/core';
import { PostsService} from './posts.service';
import { AccountService } from '../start-page/account.service';
import { Post} from '../post/Itpost';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { PostService } from '../post/post.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-feed-posts',
  templateUrl: './feed-posts.component.html',
  styleUrls: ['./feed-posts.component.sass']
})
export class FeedPostsComponent implements OnInit {
  isAdmin = false;
  currentUser;
  // postsData: post[];

  @Input() postsData: Post[];

  constructor(
    private accountService: AccountService,
    private postService: PostService,
    private postsService: PostsService,
    private toastr: ToastrService,
    private router: Router
    ) { }

  ngOnInit() {
    const role = this.accountService.getRole();
    this.isAdmin = role === 'Admin';
    this.accountService.GetUserProfile().subscribe(res => {
      this.currentUser = res;
  }
    );

  }

  isOwnPost(id) {
    if (id[0] === this.currentUser.id) {
      return true;
    }
    return false;
  }

  deletePost(id, index) {
    this.postService.deletePost(id[0]).subscribe((res: any) => {
      this.toastr.success('Пост удален');
      this.postsData.splice(index, 1);
    }, (errorResponse: HttpErrorResponse) => {
      this.toastr.error('Ошибка удаления поста');
    });
  }
  like(post: Post) {
    if (post.liked) {
      post.likesCount--;
      post.liked = false;
      this.postService.unlike(post.id).subscribe(x => {});
      return;
    }
    post.likesCount++;
    post.liked = true;
    this.postService.like(post.id).subscribe(x => {});
  }



  save(post: Post) {
    if (post.saved) {
      post.savesCount--;
      post.saved = false;
      this.postService.unsave(post.id).subscribe(x => {});
      return;
    }
    post.savesCount++;
    post.saved = true;
    this.postService.save(post.id).subscribe(x => {});
  }
}
