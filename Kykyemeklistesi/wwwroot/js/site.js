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

// Reklam Lazy Load Sistemi
function initAdLazyLoad() {
    if ('IntersectionObserver' in window) {
        const adObserver = new IntersectionObserver((entries, observer) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    loadAds();
                    observer.disconnect(); // Script yüklendiğinde gözlemlemeyi bırak
                }
            });
        }, { rootMargin: '800px' }); // Kullanıcı reklam alanına 800px yaklaşınca yükle

        // Sayfada reklam alanı varsa gözlemlemeye başla
        const adElements = document.querySelectorAll('.ad-banner, ins.adsbygoogle');
        if (adElements.length > 0) {
            adObserver.observe(adElements[0]);
        }
    } else {
        // Observer desteklenmiyorsa hemen yükle
        loadAds();
    }
}

function loadAds() {
    if (window.adsLoaded) return;
    window.adsLoaded = true;

    console.log("Reklamlar yükleniyor...");

    // AdSense Scriptini Yükle
    const adsenseScript = document.createElement('script');
    adsenseScript.async = true;
    adsenseScript.src = "https://pagead2.googlesyndication.com/pagead/js/adsbygoogle.js?client=ca-pub-5290089711270021";
    adsenseScript.crossOrigin = "anonymous";
    document.head.appendChild(adsenseScript);

    // Her bir reklam birimini tetikle
    document.querySelectorAll('ins.adsbygoogle').forEach(ins => {
        (window.adsbygoogle = window.adsbygoogle || []).push({});
    });
}

// Sayfa yüklendiğinde başlat
document.addEventListener('DOMContentLoaded', initAdLazyLoad);
