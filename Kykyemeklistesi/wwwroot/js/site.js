$(document).ready(function () {
    console.log("Kykyemeklistesi JS yüklendi.");

    $("#excel").on("click", function () {
        $(this).append('<span class="loading-text ms-2">İndiriliyor...</span>');
        setTimeout(() => {
            $(".loading-text").fadeOut(500, function () { $(this).remove(); });
        }, 3000);
    });

    $("#excel").on("click", function () {
        $(this).addClass("shake");

        setTimeout(() => {
            $(this).removeClass("shake");
        }, 500);
    });
});

function oyver(yemekId, isLike, element) {
    const votingSection = element.closest('.voting-section');
    const buttons = votingSection.querySelectorAll('.btn-vote');
    
    // Butonları geçici olarak devre dışı bırak
    buttons.forEach(btn => btn.disabled = true);

    fetch('/Home/Oyla', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
        },
        body: `yemekId=${yemekId}&isLike=${isLike}`
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Sunucu hatası: ' + response.status);
        }
        return response.json();
    })
    .then(data => {
        if (data.success) {
            // Sayıları güncelle
            votingSection.querySelector('.like-count').innerText = data.likes;
            votingSection.querySelector('.dislike-count').innerText = data.dislikes;
            
            // Aktif butonu işaretle
            element.classList.add('active');
            
            // Diğer butonu pasif yap
            buttons.forEach(btn => {
                if (btn !== element) btn.classList.remove('active');
            });

            showToast("Oyunuz başarıyla kaydedildi!", "success");
        } else {
            // Güncel sayıları yine de gösterelim (varsa)
            if (data.likes !== undefined) votingSection.querySelector('.like-count').innerText = data.likes;
            if (data.dislikes !== undefined) votingSection.querySelector('.dislike-count').innerText = data.dislikes;
            
            showToast(data.message, "warning");
        }
    })
    .catch(error => {
        console.error('Hata:', error);
        showToast("Oylama yapılamadı. Lütfen tekrar deneyin.", "error");
        buttons.forEach(btn => btn.disabled = false);
    });
}

function showToast(message, type = "success") {
    const container = document.getElementById('toast-container');
    if (!container) return;

    const toast = document.createElement('div');
    toast.className = `custom-toast ${type}`;
    
    let icon = "bi-check-circle-fill";
    if (type === "error") icon = "bi-exclamation-circle-fill";
    if (type === "warning") icon = "bi-exclamation-triangle-fill";

    toast.innerHTML = `
        <i class="bi ${icon}"></i>
        <span>${message}</span>
    `;

    container.appendChild(toast);

    // Animasyonla göster
    setTimeout(() => toast.classList.add('show'), 100);

    // 4 saniye sonra kaldır
    setTimeout(() => {
        toast.classList.remove('show');
        setTimeout(() => toast.remove(), 300);
    }, 4000);
}
