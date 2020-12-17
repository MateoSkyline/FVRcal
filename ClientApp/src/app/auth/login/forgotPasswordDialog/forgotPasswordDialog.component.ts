import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { UserService } from '../../../shared/user.service';

export interface DialogData {
  email: string
}

@Component({
  selector: 'forgotPasswordDialog',
  templateUrl: './forgotPasswordDialog.component.html',
  styleUrls: ['./forgotPasswordDialog.component.css']
})

export class ForgotPasswordDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<ForgotPasswordDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public service: UserService
  ) { }

  onSubmit() {
    this.dialogRef.close({ email: this.service.forgotPasswordFormModel.value.Email });
  }

}
