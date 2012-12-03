function siftmatch()

sift1 = rand(1000, 128);
sift2 = rand(1000, 128);

    function index = match(i)
        dists = norm(repmat(sift1(i, :), 1000, 1) - sift2);
        [~, index] = max(dists); 
    end

tic;
matches = arrayfun(@match, 1: 1000);
toc;
end