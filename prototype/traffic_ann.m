
function traffic_ann()
    % load training data
    input_data = xlsread('../data/input.csv');
    target_data = xlsread('../data/target.csv')';
    
    % prepare ANN
    ann = feedforwardnet(100);
    ann.trainParam.epochs = 1000;
    ann.trainFcn = 'traingd';
    ann.performFcn = 'mse';
    ann.trainParam.showCommandLine = true;
    
    % training time!
    [ann, tr] = train(ann, input_data', target_data);
    
    % test inputs
    test = input_data(1:6,:);               % get first set
    str_test = sprintf('%f %f\n', test);    % stringified
    fprintf('Test inputs:\n%s\n', str_test)
    
    output = ann(test');                    % raw output
    output = round(output, 1);              % rounded output
    output = sprintf('%f\n', output);       % stringified output
    fprintf('Output values:\n%s\n', output)
    
    expected = target_data(1:6);            % get first set
    
    
    % time to apply to traffic lights
    % NOTE: do Ctrl+C on command line window to stop
    pause on;
    
    prev = 0.6;
    current = 0.1;
    
    while true
        
        next = ann([prev; current]);
        next = round(next, 1);
        fprintf('%f %f %f\n', prev, current, next);
        
        switch(next)
            case 0.1 
                img_to_show = '../img/crossroadRG.jpg';
            case 0.2 
                img_to_show = '../img/crossroadRA.jpg';
            case 0.3 
                img_to_show = '../img/crossroadRR.jpg';
            case 0.4 
                img_to_show = '../img/crossroadGR.jpg';
            case 0.5 
                img_to_show = '../img/crossroadAR.jpg';
            case 0.6 
                img_to_show = '../img/crossroadRR.jpg';
            otherwise
                img_to_show = '../img/crossroadRR.jpg';
        end
        
        a = imread( img_to_show );
        imshow(a); 
        drawnow;
        pause(1);
        
        prev = current;
        current = next;
    end
    
end