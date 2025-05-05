document.addEventListener("DOMContentLoaded", () => {
    initQuills()
    initModals()
    initDropdowns()
    initCustomSelects()
    initEditModalTriggers()
    initDeleteButtons()
    initEmailChecker()
})

function initQuills() {
    document.querySelectorAll('[data-quill-editor]').forEach(editor => {
        const editorId = editor.id
        const textarea = document.querySelector(`[data-quill-textarea="#${editorId}"]`)
        const toolbarId = editor.getAttribute('data-quill-toolbar')

        const quill = new Quill(`#${editorId}`, {
            modules: {
                syntax: true,
                toolbar: toolbarId
            },
            theme: 'snow',
            placeholder: 'Type something'
        })

        if (textarea) {
            quill.on('text-change', () => {
                textarea.value = quill.root.innerHTML
            })

            quill.root.innerHTML = textarea.value
        }
    })
}

function initModals() {
    document.querySelectorAll('[data-type="modal"]').forEach(trigger => {
        const target = document.querySelector(trigger.getAttribute('data-target'))
        trigger.addEventListener('click', () => {
            target?.classList.add('modal-show')
        })
    })

    document.querySelectorAll('[data-type="close"]').forEach(btn => {
        const target = document.querySelector(btn.getAttribute('data-target'))
        btn.addEventListener('click', () => {
            target?.classList.remove('modal-show')
        })
    })
}

function initDropdowns() {
    document.addEventListener('click', (e) => {
        let clickedInsideDropdown = false

        document.querySelectorAll('[data-type="dropdown"]').forEach(dropdownTrigger => {
            const targetId = dropdownTrigger.getAttribute('data-target')
            const dropdown = document.querySelector(targetId)

            if (dropdownTrigger.contains(e.target)) {
                clickedInsideDropdown = true
                document.querySelectorAll('.dropdown.dropdown-show').forEach(open => {
                    if (open !== dropdown) open.classList.remove('dropdown-show')
                })
                dropdown?.classList.toggle('dropdown-show')
            }
        })

        if (!clickedInsideDropdown && !e.target.closest('.dropdown')) {
            document.querySelectorAll('.dropdown.dropdown-show').forEach(open => {
                open.classList.remove('dropdown-show')
            })
        }
    })
}

function initCustomSelects() {
    document.querySelectorAll('.form-select').forEach(select => {
        const trigger = select.querySelector('.form-select-trigger')
        const triggerText = trigger.querySelector('.form-select-text')
        const options = select.querySelectorAll('.form-select-option')
        const hiddenInput = select.querySelector('input[type="hidden"]')
        const placeholder = select.dataset.placeholder || "Choose"

        const setValue = (value = "", text = placeholder) => {
            triggerText.textContent = text
            hiddenInput.value = value
            select.classList.toggle('has-placeholder', !value)
        }

        const initialValue = hiddenInput.value
        if (initialValue) {
            const matchedOption = Array.from(options).find(option => option.dataset.value === initialValue)
            if (matchedOption) setValue(initialValue, matchedOption.textContent)
        } else {
            setValue()
        }

        trigger.addEventListener('click', e => {
            e.stopPropagation()
            document.querySelectorAll('.form-select.open').forEach(el => {
                if (el !== select) el.classList.remove('open')
            })
            select.classList.toggle('open')
        })

        options.forEach(option => {
            option.addEventListener('click', () => {
                setValue(option.dataset.value, option.textContent)
                select.classList.remove('open')
            })
        })

        document.addEventListener('click', e => {
            if (!select.contains(e.target)) select.classList.remove('open')
        })
    })
}

function initEditModalTriggers() {
    document.querySelectorAll('[data-type="modal"][data-id]').forEach(button => {
        button.addEventListener('click', async () => {
            const projectId = button.getAttribute('data-id')
            const modal = document.querySelector('#edit-project-modal')

            if (modal) {
                try {
                    const response = await fetch(`/Projects/Edit/${projectId}`)
                    const html = await response.text()

                    modal.outerHTML = html

                    const newModal = document.querySelector('#edit-project-modal')
                    newModal?.classList.add('modal-show')

                    initQuills()
                    initCustomSelects()
                    initModals()
                } catch (err) {
                    console.error('Failed to load edit form', err)
                }
            }
        })
    })
}

function initDeleteButtons() {
    document.querySelectorAll('.dropdown-action.remove[data-id]').forEach(button => {
        button.addEventListener('click', (e) => {
            e.preventDefault()
            const projectId = button.getAttribute('data-id')

            if (confirm('Are you sure you want to delete this project?')) {
                const form = document.createElement('form')
                form.method = 'post'
                form.action = `/Projects/Delete/${projectId}`

                const token = document.querySelector('input[name="__RequestVerificationToken"]')
                if (token) {
                    const hidden = document.createElement('input')
                    hidden.type = 'hidden'
                    hidden.name = '__RequestVerificationToken'
                    hidden.value = token.value
                    form.appendChild(hidden)
                }

                document.body.appendChild(form)
                form.submit()
            }
        })
    })
}
