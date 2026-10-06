// site.js — Global JavaScript for Hospital Management System

document.addEventListener('DOMContentLoaded', function () {

    // ============================================================
    // 1. Process TempData messages and display as SweetAlert2
    // ============================================================

    const successEl = document.getElementById('tempdata-success');
    if (successEl) {
        Swal.fire({
            icon: 'success',
            title: 'Success!',
            text: successEl.getAttribute('data-message'),
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 3500,
            timerProgressBar: true
        });
    }

    const errorEl = document.getElementById('tempdata-error');
    if (errorEl) {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: errorEl.getAttribute('data-message'),
            confirmButtonColor: '#0d6efd'
        });
    }

    const warningEl = document.getElementById('tempdata-warning');
    if (warningEl) {
        Swal.fire({
            icon: 'warning',
            title: 'Notice',
            text: warningEl.getAttribute('data-message'),
            confirmButtonColor: '#ffc107'
        });
    }

    const registerSuccessEl = document.getElementById('tempdata-register-success');
    if (registerSuccessEl) {
        Swal.fire({
            icon: 'success',
            title: '🎉 Registration Successful!',
            text: registerSuccessEl.getAttribute('data-message'),
            confirmButtonColor: '#0d6efd',
            confirmButtonText: 'Sign In Now'
        });
    }

    const logoutEl = document.getElementById('tempdata-logout');
    if (logoutEl) {
        Swal.fire({
            icon: 'info',
            title: 'Logged Out',
            text: logoutEl.getAttribute('data-message'),
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true
        });
    }

    // ============================================================
    // 2. Add animation class to main content cards on page load
    // ============================================================
    document.querySelectorAll('.stat-card').forEach((card, index) => {
        card.classList.add('animate-in');
        card.style.animationDelay = `${index * 0.1}s`;
        card.style.opacity = '0';
        setTimeout(() => { card.style.opacity = ''; }, 50);
    });

    // ============================================================
    // 3. Active nav-link highlighting based on current URL
    // ============================================================
    const currentPath = window.location.pathname.toLowerCase();
    document.querySelectorAll('.navbar .nav-link').forEach(link => {
        const href = link.getAttribute('href');
        if (href && currentPath.startsWith(href.toLowerCase()) && href !== '/') {
            link.classList.add('active');
        }
    });

    // ============================================================
    // 4. Auto-dismiss Bootstrap alerts after 5 seconds
    // ============================================================
    document.querySelectorAll('.alert.alert-dismissible').forEach(alert => {
        setTimeout(() => {
            const bsAlert = bootstrap.Alert.getOrCreateInstance(alert);
            if (bsAlert) bsAlert.close();
        }, 5000);
    });

});
