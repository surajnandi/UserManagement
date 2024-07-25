import { Component, OnInit, ViewChild } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { UserService } from './user.service';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule, HttpClientModule, FormsModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  providers: [UserService],
})
export class AppComponent {
  title = 'UserManagement';

  users: any[] = [];

  @ViewChild('userForm') userForm!: NgForm;
  newUser: any = {
    userName: '',
    firstName: '',
    lastName: '',
    email: '',
    phone: '',
    address: '',
    password: '',
  };

  searchEmail: string = '';
  private debounceTimeout: any;
  isSubmitting = false;
  confirmPassword: string = '';
  existingUser: any = {};
  errorMessage: string = '';
  toastMessage: string = '';
  showToast: boolean = false;

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.userService.getUsers().subscribe(
      (data) => {
        this.users = data;
      },
      (error) => {
        console.error('Error fetching user data', error);
      }
    );
  }

  fetchUserById(id: number): void {
    this.userService.getUserById(id).subscribe(
      (user) => {
        this.existingUser = user;
        this.newUser = { ...this.existingUser };
      },
      (error) => {
        console.error('Error fetching user:', error);
      }
    );
  }

  onSearchChange(term: string): void {
    if (!term) {
      clearTimeout(this.debounceTimeout);
      this.loadUsers();
      return;
    }
    clearTimeout(this.debounceTimeout);
    this.debounceTimeout = setTimeout(() => {
      this.userService.searchUsersByEmail(term).subscribe((user) => {
        this.users = user;
      });
    }, 300); // wait 300ms after each keystroke
  }

  addNewUser(): void {
    if (this.newUser.password !== this.confirmPassword) {
      this.errorMessage = 'Passwords do not match';
      return;
    }
    this.isSubmitting = true;
    this.errorMessage = '';
    this.userService
      .createUser(this.newUser)
      .subscribe(
        (response) => {
          console.log('User created successfully:', response);
          this.closeModal();
          this.loadUsers();
          this.showToastMessage('User created successfully!');
          // window.location.reload();
        },
        (error) => {
          console.error('Error creating user:', error);
          this.errorMessage = error.error
            ? error.error
            : 'An unknown error occurred';
        }
      )
      .add(() => {
        this.isSubmitting = false;
      });
  }

  updateUser(): void {
    if (this.newUser.password !== this.confirmPassword) {
      this.errorMessage = 'Passwords do not match';
      return;
    }
    this.isSubmitting = true;
    this.errorMessage = '';
    this.userService
      .updateUser(this.existingUser.id, this.newUser)
      .subscribe(
        (response) => {
          console.log('User updated successfully!');
          this.closeUpdateModal();
          this.loadUsers();
          // window.location.reload();
          this.showToastMessage('User updated successfully!');
        },
        (error) => {
          console.error('Error updating user:', error);
          this.errorMessage = error.error
            ? error.error
            : 'An unknown error occurred';
        }
      )
      .add(() => {
        this.isSubmitting = false;
      });
  }

  deleteUser(id: number): void {
    this.userService.deleteUser(id).subscribe(() => {
      this.loadUsers();
      this.showToastMessage('User deleted successfully!');
    });
  }

  activeDeactive(id: number): void {
    this.userService.activeDeactive(id).subscribe(() => {
      this.loadUsers();
    });
  }

  closeModal(): void {
    const modal = document.getElementById('addUserModal');
    if (modal) {
      modal.classList.remove('show');
      document.body.classList.remove('modal-open');
      document.body.style.paddingRight = '';
      const backdrop = document.querySelector('.modal-backdrop');
      if (backdrop) {
        backdrop.remove();
      }
    }
  }

  closeUpdateModal(): void {
    this.errorMessage = '';
    const modal = document.getElementById('updateUserModal');
    if (modal) {
      modal.classList.remove('show');
      document.body.classList.remove('modal-open');
      document.body.style.paddingRight = '';
      const backdrop = document.querySelector('.modal-backdrop');
      if (backdrop) {
        backdrop.remove();
      }
    }
  }

  discardChanges(): void {
    this.errorMessage = '';
    if (this.userForm) {
      this.closeModal();
      this.closeUpdateModal();
      this.loadUsers();
      this.errorMessage = '';
    }
  }
  passwordsMatch(): boolean {
    return this.newUser.password === this.confirmPassword;
  }

  showToastMessage(message: string) {
    this.toastMessage = message;
    this.showToast = true;
    setTimeout(() => {
      this.showToast = false;
      window.location.reload();
    }, 3000);
  }

  onToastHidden() {
    this.showToast = false;
  }
}
