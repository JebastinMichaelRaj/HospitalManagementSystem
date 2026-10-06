document.addEventListener('DOMContentLoaded', function () {
    // Select the modal elements from _Layout.cshtml
    const modalElement = document.getElementById('formModal');
    const modalBody = document.getElementById('modal-body-content');
    const modalTitle = document.getElementById('formModalLabel');

    if (!modalElement) return; // Guard if modal doesn't exist on page

    // Initialize the Bootstrap Modal instance
    const bsModal = new bootstrap.Modal(modalElement);

    // Listen for clicks on ANY button/link with class 'open-modal-btn'
    document.querySelectorAll('.open-modal-btn').forEach(button => {
        button.addEventListener('click', function (e) {
            e.preventDefault();

            const url = this.getAttribute('data-url');
            const title = this.getAttribute('data-title');

            if (title) modalTitle.textContent = title;

            // Show loading spinner while fetching partial view
            modalBody.innerHTML = `
                <div class="text-center py-4">
                    <div class="spinner-border text-primary" role="status"></div>
                    <p class="mt-2 text-muted">Loading...</p>
                </div>`;
            bsModal.show();

            fetch(url)
                .then(response => {
                    if (!response.ok) throw new Error(`Server responded with ${response.status}`);
                    return response.text();
                })
                .then(html => {
                    modalBody.innerHTML = html;
                    attachFormSubmitListener(modalBody, bsModal);
                })
                .catch(error => {
                    bsModal.hide();
                    Swal.fire({
                        icon: 'error',
                        title: 'Failed to Load',
                        text: 'Could not load the form. Please try again.',
                        confirmButtonColor: '#0d6efd'
                    });
                    console.error('Error fetching modal content:', error);
                });
        });
    });

    // Handle form submission inside the modal via AJAX
    function attachFormSubmitListener(container, modalInstance) {
        const form = container.querySelector('form');
        if (!form) return;

        form.addEventListener('submit', function (e) {
            e.preventDefault();

            const submitBtn = form.querySelector('[type="submit"]');
            const originalText = submitBtn ? submitBtn.innerHTML : '';

            // Show loading state on submit button
            if (submitBtn) {
                submitBtn.disabled = true;
                submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Saving...';
            }

            const actionUrl = form.getAttribute('action');
            const formData = new FormData(form);

            fetch(actionUrl, {
                method: 'POST',
                body: formData
            })
                .then(async response => {
                    if (response.ok) {
                        let successMessage = 'Record saved successfully!';
                        try {
                            const data = await response.json();
                            if (data && data.message) successMessage = data.message;
                        } catch (e) {
                            // Response wasn't JSON, use default message
                        }

                        // Hide modal, then show SweetAlert success toast
                        modalInstance.hide();
                        Swal.fire({
                            icon: 'success',
                            title: 'Success!',
                            text: successMessage,
                            toast: true,
                            position: 'top-end',
                            showConfirmButton: false,
                            timer: 3000,
                            timerProgressBar: true,
                            didOpen: (toast) => {
                                toast.addEventListener('mouseenter', Swal.stopTimer);
                                toast.addEventListener('mouseleave', Swal.resumeTimer);
                            }
                        });

                        // Reload the page after showing the toast
                        setTimeout(() => window.location.reload(), 1500);

                    } else {
                        // Validation failed — re-render form with errors
                        const html = await response.text();
                        container.innerHTML = html;
                        attachFormSubmitListener(container, modalInstance);

                        if (submitBtn) {
                            submitBtn.disabled = false;
                            submitBtn.innerHTML = originalText;
                        }
                    }
                })
                .catch(error => {
                    if (submitBtn) {
                        submitBtn.disabled = false;
                        submitBtn.innerHTML = originalText;
                    }
                    Swal.fire({
                        icon: 'error',
                        title: 'Submission Failed',
                        text: 'An unexpected error occurred. Please try again.',
                        confirmButtonColor: '#0d6efd'
                    });
                    console.error('Error submitting form:', error);
                });
        });
    }

    // Handle SweetAlert2 confirm dialog for "Mark as Paid" POST forms
    document.querySelectorAll('.confirm-action-form').forEach(form => {
        form.addEventListener('submit', function (e) {
            e.preventDefault();
            const message = this.getAttribute('data-confirm-message') || 'Are you sure?';
            const title = this.getAttribute('data-confirm-title') || 'Confirm Action';

            Swal.fire({
                title: title,
                text: message,
                icon: 'question',
                showCancelButton: true,
                confirmButtonColor: '#198754',
                cancelButtonColor: '#6c757d',
                confirmButtonText: 'Yes, proceed!',
                cancelButtonText: 'Cancel'
            }).then((result) => {
                if (result.isConfirmed) {
                    form.submit();
                }
            });
        });
    });
});