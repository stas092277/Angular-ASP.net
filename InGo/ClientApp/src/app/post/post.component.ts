import { Component, OnInit } from '@angular/core';
import { Post } from './Itpost';
import { AccountService } from '../start-page/account.service';
import { PostService } from './post.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { HttpClient, HttpEventType, HttpErrorResponse } from '@angular/common/http';


@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.sass']
})
export class PostComponent implements OnInit {
  defaultValue = '';
  userDetails;
  post: Post;
  isAdmin = false;

  constructor(
    private accountService: AccountService,
    private route: ActivatedRoute,
    private postService: PostService,
    private toastr: ToastrService,
    private router: Router
    ) { }

  ngOnInit() {
    const id = +this.route.snapshot.paramMap.get('id');
    this.postService.getDataPostById(id).subscribe(data => this.post = data,
      err => console.log('HTTP error', err.message));

    this.accountService.GetUserProfile().subscribe(res => {
      this.userDetails = res;
    }, err => { console.log(err); }, );

    const role = this.accountService.getRole();
    this.isAdmin = role === 'Admin';
}

  isOwnPost(id) {
    return id[0] === this.userDetails.id;
  }

  deletePost(id) {
    this.postService.deletePost(id[0]).subscribe((res: any) => {
      this.toastr.success('Пост Удален');
      this.router.navigateByUrl('/posts');
    }, (errorResponse: HttpErrorResponse) =>{
      this.toastr.error('Ошибка удаления поста');
    }
    );
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
