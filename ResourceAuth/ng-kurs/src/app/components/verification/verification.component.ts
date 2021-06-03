import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AccountService } from 'src/app/services/account.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-verification',
  templateUrl: './verification.component.html',
  styleUrls: ['./verification.component.scss']
})
export class VerificationComponent implements OnInit {

  userId: number;
  userCode: string;
  error:boolean=false;
  spiner:boolean=true;
  constructor(private route: ActivatedRoute,private acc:AuthService) { 
    
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => { const userId = params['userId']; const userCode = params['code']; this.acc.VerifyAccount(userId, userCode).subscribe(res => { this.spiner = false }, err => { this.error = true, this.spiner = false }) });
  
  }

}
