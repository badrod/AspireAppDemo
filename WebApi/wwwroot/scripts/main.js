
   // Pretty-print JSON and add simple syntax highlighting
    function syntaxHighlight(json) {
            if (typeof json !== 'string') {
        json = JSON.stringify(json, undefined, 2);
            }
    json = json
    .replace(/&/g, '&amp;')
    .replace(/</g, '&lt;')
                .replace(/>/g, '&gt;');

return json.replace(/("(\\u[a-zA-Z0-9]{4}|\\[^u]|[^\\"])*"(?:\s*:)?|\b(true|false|null)\b|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?)/g,
    function (match) {
        let cls = 'number';
        if (/^"/.test(match)) {
            if (/:$/.test(match)) {
                cls = 'key';
            } else {
                cls = 'string';
            }
        } else if (/true|false/.test(match)) {
            cls = 'boolean';
        } else if (/null/.test(match)) {
            cls = 'null';
        }
        return '<span class="' + cls + '">' + match + '</span>';
    });
        }

// Show preview for selected file (image or PDF)
const fileInput = document.getElementById('fileInput');
const previewEl = document.getElementById('preview');
const resultPre = document.getElementById('result');

function clearPreview() {
    // revoke any object URLs created
    const objs = previewEl.querySelectorAll('[data-obj-url]');
    objs.forEach(el => {
        const url = el.getAttribute('data-obj-url');
        try { URL.revokeObjectURL(url); } catch { }
    });
    previewEl.innerHTML = '';
}

function showPreview(file) {
    clearPreview();
    if (!file) return;

    const box = document.createElement('div');
    box.className = 'preview-box';

    const meta = document.createElement('div');
    meta.className = 'preview-meta';
    meta.textContent = `${file.name} — ${(file.size / 1024).toFixed(1)} KB — ${file.type || 'n/a'}`;

    if (file.type.startsWith('image/')) {
        const img = document.createElement('img');
        img.className = 'preview-img';
        const url = URL.createObjectURL(file);
        img.src = url;
        img.setAttribute('data-obj-url', url);
        // attach attribute to revoke later
        img.onload = () => { try { URL.revokeObjectURL(url); } catch { } };
        box.appendChild(img);
        box.appendChild(meta);
        previewEl.appendChild(box);
    } else if (file.type === 'application/pdf' || file.name.toLowerCase().endsWith('.pdf')) {
        const info = document.createElement('div');
        info.textContent = 'PDF preview (embedded). If your browser cannot render PDFs, the file can still be uploaded.';
        info.style.marginBottom = '8px';

        const iframe = document.createElement('iframe');
        iframe.className = 'preview-pdf';
        const url = URL.createObjectURL(file);
        iframe.src = url;
        iframe.setAttribute('data-obj-url', url);
        // revoke when iframe loads (may not fire in all browsers reliably, so still revoke on clear)
        iframe.onload = () => { try { URL.revokeObjectURL(url); } catch { } };

        box.appendChild(info);
        box.appendChild(iframe);
        box.appendChild(meta);
        previewEl.appendChild(box);
    } else {
        const note = document.createElement('div');
        note.textContent = 'No preview available for this file type.';
        box.appendChild(note);
        box.appendChild(meta);
        previewEl.appendChild(box);
    }
}

fileInput.addEventListener('change', function () {
    const f = fileInput.files && fileInput.files[0];
    showPreview(f || null);
    // clear previous result when picking a new file
    resultPre.textContent = '';
});

document.getElementById('uploadForm').addEventListener('submit', async function (e) {
    e.preventDefault();
    const input = document.getElementById('fileInput');
    const file = input.files && input.files[0];
    const resultPre = document.getElementById('result');

    if (!file) {
        resultPre.innerHTML = '<span class="error">Please choose a file.</span>';
        return;
    }

    const fd = new FormData();
    fd.append('file', file);

    try {
        const resp = await fetch('/api/upload', {
            method: 'POST',
            body: fd
        });

        const json = await resp.json();
        if (!resp.ok) {
            resultPre.innerHTML = '<span class="error">' + (json.error || 'Upload failed') + '</span>';
            return;
        }

        resultPre.innerHTML = syntaxHighlight(json);
    } catch (err) {
        resultPre.innerHTML = '<span class="error">Network error: ' + err.message + '</span>';
    }
});

// revoke object URLs on page unload to avoid leaks
window.addEventListener('beforeunload', clearPreview);
   